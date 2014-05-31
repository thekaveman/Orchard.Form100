using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CSM.Form100.Collections
{
    /// <summary>
    /// The possible ordering types for an OrderedCollection
    /// </summary>
    public enum CollectionOrder { Auto, FIFO, LIFO }

    /// <summary>
    /// A generic container class that provides access to memebers in an ordered manner.
    /// </summary>
    public abstract class OrderedCollection<T> : IEnumerable<T>
    {
        /// <summary>
        /// A container for each of the ordering types
        /// </summary>
        protected List<T>  list  { get; private set; }
        protected Queue<T> queue { get; private set; }
        protected Stack<T> stack { get; private set; }

        public OrderedCollection(CollectionOrder order, IEnumerable<T> initialCollection = null)
        {
            Order = order;

            //initialize the underlying container based on order choice

            decide(
                () => list = new List<T>(initialCollection),
                () => queue = new Queue<T>(initialCollection),
                () => stack = new Stack<T>(initialCollection)
            );
        }

        public CollectionOrder Order { get; private set; }

        /// <summary>
        /// Adds an item to the collection, respecting the defined Order.
        /// </summary>
        /// <param name="item">The item of type T to add.</param>
        public virtual void Add(T item)
        {
            //add to the appropriate container

            decide(
                () => list.Add(item),
                () => queue.Enqueue(item),
                () => stack.Push(item)
            );
        }

        /// <summary>
        /// Removes and returns the next item in the collection, respecting the defined Order.
        /// </summary>
        /// <returns>The next item in the collection, or default(T).</returns>
        public virtual T RemoveNext()
        {
            T next = default(T);

            //ensure there is an item to return

            if (this.Any())
            {
                //get the next item from the appropriate container

                next = decide(
                    () => list.First(),
                    () => queue.Dequeue(),
                    () => stack.Pop()
                );
            }

            return next;
        }

        /// <summary>
        /// Return the next item in the collection without removing it, respecting the defined Order.
        /// </summary>
        /// <returns>The next item in the collection, or default(T).</returns>
        public virtual T PeekNext()
        {
            T next = default(T);

            //ensure there is an item to return

            if (this.Any())
            {
                //get the next item from the appropriate container

                next = decide(
                    () => list.First(),
                    () => queue.Peek(),
                    () => stack.Peek()
                );
            }

            return next;
        }
        
        /// <summary>
        /// Call one of the provided functions, based on the defined Order.
        /// </summary>
        protected void decide(Action listAction, Action queueAction, Action stackAction)
        {
            if (Order == CollectionOrder.FIFO)
                queueAction();
            else if (Order == CollectionOrder.LIFO)
                stackAction();
            else
                listAction();
        }

        /// <summary>
        /// Return the result of one of the provided functions, based on the defined Order.
        /// </summary>
        /// <typeparam name="TResult">The return type of each provided function.</typeparam>
        protected TResult decide<TResult>(Func<TResult> listFunction, Func<TResult> queueFunction, Func<TResult> stackFunction)
        {
            if (Order == CollectionOrder.FIFO)
                return queueFunction();
            else if (Order == CollectionOrder.LIFO)
                return stackFunction();
            else
                return listFunction();
        }

        #region IEnumerable<T> implementation

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            //proxy the call to the appropriate container

            return decide(
                () => list.GetEnumerator(),
                () => queue.AsEnumerable().GetEnumerator(),
                () => stack.AsEnumerable().GetEnumerator()
            );
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            //at call time, T will have resolved into a formal type

            return (this as IEnumerable<T>).GetEnumerator();
        }

        #endregion
    }
}