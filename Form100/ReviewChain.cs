using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CSM.Form100.Models;
using CSM.Form100.Services;

namespace CSM.Form100
{
    public class ReviewChain : OrderedCollection<ReviewStepRecord>
    {
        private ReviewPartRecord target;
        private IReviewService reviewService;

        public ReviewChain(CollectionOrder order, ReviewPartRecord target, IReviewService reviewService, IEnumerable<ReviewStepRecord> initialCollection = null)
            : base(order, initialCollection)
        {
            this.target = target;
            this.reviewService = reviewService;
        }

        public override void Add(ReviewStepRecord item)
        {
            if (item != default(ReviewStepRecord))
            {
                base.Add(item);
                serializeToTarget();
            }
        }

        public override ReviewStepRecord RemoveNext()
        {
            var next = base.RemoveNext();

            if(next != default(ReviewStepRecord))
                serializeToTarget();

            return next;
        }

        public ReviewChain Clone()
        {
            return new ReviewChain(Order, null, null, this.AsEnumerable());
        }
        
        private void serializeToTarget()
        {
            if (target != null && reviewService != null)
                decide(
                    () => { },
                    () => target.PendingReviewsIds = reviewService.SerializeReviewStepIds(queue as Queue<ReviewStepRecord>),
                    () => target.ReviewHistoryIds = reviewService.SerializeReviewStepIds(stack as Stack<ReviewStepRecord>)
                );
        }
    }
}