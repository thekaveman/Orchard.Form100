using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CSM.Form100
{
    public enum CollectionOrder { Auto, FIFO, LIFO }

    public abstract class OrderedCollection<T> : IEnumerable<T>
    {
        protected List<T>  list  { get; private set; }
        protected Queue<T> queue { get; private set; }
        protected Stack<T> stack { get; private set; }

        public OrderedCollection(CollectionOrder order, IEnumerable<T> initialCollection)
        {
            Order = order;

            decide(
                () => list = new List<T>(initialCollection),
                () => queue = new Queue<T>(initialCollection),
                () => stack = new Stack<T>(initialCollection)
            );
        }

        public CollectionOrder Order { get; private set; }

        public virtual void Add(T item)
        {
            decide(
                () => list.Add(item),
                () => queue.Enqueue(item),
                () => stack.Push(item)
            );
        }

        public virtual T RemoveNext()
        {
            T next = default(T);

            if (this.Any())
            {
                next = decide(
                    () => list.First(),
                    () => queue.Dequeue(),
                    () => stack.Pop()
                );
            }

            return next;
        }

        public virtual T PeekNext()
        {
            T next = default(T);

            if (this.Any())
            {
                next = decide(
                    () => list.First(),
                    () => queue.Peek(),
                    () => stack.Peek()
                );
            }

            return next;
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return decide(
                () => list.GetEnumerator(),
                () => queue.AsEnumerable().GetEnumerator(),
                () => stack.AsEnumerable().GetEnumerator()
            );
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (this as IEnumerable<T>).GetEnumerator();
        }

        protected void decide(Action listAction, Action queueAction, Action stackAction)
        {
            if (Order == CollectionOrder.FIFO)
                queueAction();
            else if (Order == CollectionOrder.LIFO)
                stackAction();
            else
                listAction();
        }

        protected TResult decide<TResult>(Func<TResult> listFunction, Func<TResult> queueFunction, Func<TResult> stackFunction)
        {
            if (Order == CollectionOrder.FIFO)
                return queueFunction();
            else if (Order == CollectionOrder.LIFO)
                return stackFunction();
            else
                return listFunction();
        }
    }
}