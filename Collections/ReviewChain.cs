using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CSM.Form100.Models;
using CSM.Form100.Services;

namespace CSM.Form100.Collections
{
    /// <summary>
    /// An OrderedCollection implementation for managing sequences of ReviewStepRecords.
    /// </summary>
    public class ReviewChain : OrderedCollection<ReviewStepRecord>
    {
        //the backing record for the part containing this ReviewChain
        private ReviewPartRecord target;
        //will need to access serialization services
        private IReviewService reviewService;

        public ReviewChain(CollectionOrder order, ReviewPartRecord target, IReviewService reviewService, IEnumerable<ReviewStepRecord> initialCollection = null)
            : base(order, initialCollection)
        {
            if (!(order == CollectionOrder.FIFO || order == CollectionOrder.LIFO))
                throw new InvalidOperationException("Valid orderings for the ReviewChain class are FIFO and LIFO");

            this.target = target;
            this.reviewService = reviewService;
        }

        /// <summary>
        /// Adds a review step to the underlying collection, respecting the defined Order.
        /// </summary>
        /// <param name="item">The ReviewStepRecord to add.</param>
        public override void Add(ReviewStepRecord item)
        {
            if (item != default(ReviewStepRecord))
            {
                //add the step to the underlying collection
                base.Add(item);
                //update the backing record
                serializeToTarget();
            }
        }

        /// <summary>
        /// Removes and returns the next ReviewStepRecord in the underlying collection, respecting the defined Order.
        /// </summary>
        /// <returns>The next ReviewStepRecord in the collection, or default(ReviewStepRecord).</returns>
        public override ReviewStepRecord RemoveNext()
        {
            //remove the next item from the underlying collection
            var next = base.RemoveNext();

            if (next != default(ReviewStepRecord))
                //update the backing record
                serializeToTarget();

            return next;
        }

        /// <summary>
        /// Creates a clone for iteration purposes, disconnected from the original's backing record.
        /// </summary>
        public ReviewChain WeakClone()
        {
            return new ReviewChain(Order, null, null, this.AsEnumerable());
        }

        /// <summary>
        /// Update the backing record with the current state of the ReviewChain
        /// </summary>
        private void serializeToTarget()
        {
            if (target != null && reviewService != null)
            {
                //serialize to the appropriate record property
                decide(
                    //pass a no-op for the list argument (never used)
                    () => { },
                    //the reviewService serializes the underlying collection to a comma-separated list of ids
                    () => target.PendingReviewsIds = reviewService.SerializeReviewSteps(queue, r => r.Id),
                    //reverse the stack so that deserializing preserves the original order
                    () => target.ReviewHistoryIds = reviewService.SerializeReviewSteps(stack.Reverse(), r => r.Id)
                );
            }
        }
    }
}