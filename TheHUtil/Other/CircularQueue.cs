namespace TheHUtil
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Defines the <see cref="CircularQueue"/> generic class.
    /// </summary>
    /// <remarks>
    /// The feel of the class is nearly identical to that of a generic <see cref="Queue"/>. The exception being the addition of a method named "Cycle" which returns the next item in the queue, but internally moves it to the back of the line. Internally, it literally just gets put into a second queue.
    /// </remarks>
    /// <typeparam name="T">Can be any type.</typeparam>
    [Serializable]
    public class CircularQueue<T> : IEnumerable<T>, ICollection, IEnumerable
    {
        /// <summary>
        /// One of the 2 internal queues. This one is just the first one in use.
        /// </summary>
        private Queue<T> queue1;

        /// <summary>
        /// The other of the 2 queues.
        /// </summary>
        private Queue<T> queue2;

        /// <summary>
        /// Determines which queue is currently in use.
        /// </summary>
        private bool useQueue1;

        /// <summary>
        /// Gets the count of both internal queues.
        /// </summary>
        public int Count
        {
            get
            {
                return this.queue1.Count + this.queue2.Count;
            }
        }

        /// <summary>
        /// Returns all items in the queue to represent a complete cycle.
        /// </summary>
        public IEnumerable<T> OneCompleteCycle
        {
            get
            {
                for (var iteration = 0; iteration < this.Count; iteration++)
                {
                    yield return this.Cycle();
                }
            }
        }

        /// <summary>
        /// Gets the count of both internal queues.
        /// </summary>
        int ICollection.Count
        {
            get
            {
                return this.Count;
            }
        }

        /// <summary>
        /// Gets whether the queue is synchronized.
        /// </summary>
        /// <remarks>This always returns false.</remarks>
        bool ICollection.IsSynchronized
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets an object from the currently used internal queue that can be used for synchronizing.
        /// </summary>
        object ICollection.SyncRoot
        {
            get
            {
                ICollection queue;
                if (this.useQueue1)
                {
                    queue = this.queue1;
                }
                else
                {
                    queue = this.queue2;
                }

                return queue.SyncRoot;
            }
        }

        /// <summary>
        /// Initiailizes a new instance of the <see cref="CircularQueue"/> generic class.
        /// </summary>
        public CircularQueue()
        {
            this.queue1 = new Queue<T>();
            this.Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CircularQueue"/> generic class.
        /// </summary>
        /// <param name="capacity">The initial capacity of the queue.</param>
        public CircularQueue(int capacity)
        {
            this.queue1 = new Queue<T>(capacity);
            this.Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CircularQueue"/> generic class.
        /// </summary>
        /// <param name="items">A collection of items to populate the queue with.</param>
        public CircularQueue(IEnumerable<T> items)
        {
            this.queue1 = new Queue<T>(items);
            this.Initialize();
        }

        /// <summary>
        /// Initializes the second queue and sets which queue is to be used.
        /// </summary>
        private void Initialize()
        {
            this.useQueue1 = true;
            this.queue2 = new Queue<T>(this.queue1.Count);
        }

        /// <summary>
        /// Clears both internal queues.
        /// </summary>
        public void Clear()
        {
            this.queue1.Clear();
            this.queue2.Clear();
            this.useQueue1 = true;
        }

        /// <summary>
        /// Checks if an item is contained in the queue.
        /// </summary>
        /// <remarks>This method relies on the item's Equals method being overridden.</remarks>
        /// <param name="item">An item to check for.</param>
        /// <returns>True: the item exists at least once in the queue. False: it does not exist.</returns>
        public bool Contains(T item)
        {
            return this.queue1.Contains(item) || this.queue2.Contains(item);
        }

        /// <summary>
        /// Copies the contents of an array into another array.
        /// </summary>
        /// <param name="array">The array to copy to.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            if (this.useQueue1)
            {
                this.queue1.CopyTo(array, arrayIndex);
                this.queue2.CopyTo(array, arrayIndex);
            }
            else
            {
                this.queue2.CopyTo(array, arrayIndex);
                this.queue1.CopyTo(array, arrayIndex);
            }
        }
        
        /// <summary>
        /// Gets the next item in the queue and cycles it internally to the back of the line.
        /// </summary>
        /// <returns>The next item in the queue.</returns>
        public T Cycle()
        {
            var tempBool = this.useQueue1;
            T result = this.Dequeue();
            if (tempBool)
            {
                this.queue2.Enqueue(result);
            }
            else
            {
                this.queue1.Enqueue(result);
            }

            return result;
        }

        /// <summary>
        /// Gets the next item in the queue and removes it from the queue.
        /// </summary>
        /// <remarks>If your queue becomes empty and you don't want it to, check that you are using "Cycle" instead of this method.</remarks>
        /// <returns>The next item in the queue.</returns>
        public T Dequeue()
        {
            if (this.useQueue1)
            {
                var result = this.queue1.Dequeue();
                if (this.queue1.Count == 0)
                {
                    this.useQueue1 = false;
                }

                return result;
            }
            else
            {
                var result = this.queue2.Dequeue();
                if (this.queue2.Count == 0)
                {
                    this.useQueue1 = true;
                }

                return result;
            }

        }

        /// <summary>
        /// Adds an item to the back of the queue.
        /// </summary>
        /// <param name="item">The item to add to the queue.</param>
        public void Enqueue(T item)
        {
            if (this.useQueue1)
            {
                this.queue1.Enqueue(item);
            }
            else
            {
                this.queue2.Enqueue(item);
            }
        }

        /// <summary>
        /// Gets the next item in the queue without advancing it.
        /// </summary>
        /// <returns>The next item in the queue.</returns>
        public T Peek()
        {
            return this.useQueue1 ? this.queue1.Peek() : this.queue2.Peek();
        }

        /// <summary>
        /// Copies the contents of the entire queue to an array starting at the next item.
        /// </summary>
        /// <returns>The contents of the entire queue.</returns>
        public T[] ToArray()
        {
            var result = new T[this.Count];
            if (this.Count > 0)
            {
                if (this.useQueue1)
                {
                    this.queue1.CopyTo(result, 0);
                    this.queue2.CopyTo(result, this.queue1.Count);
                }
                else
                {
                    this.queue2.CopyTo(result, 0);
                    this.queue1.CopyTo(result, this.queue2.Count);
                }
            }

            return result;
        }

        /// <summary>
        /// Trims the queue if there is 10% or more unused space.
        /// </summary>
        public void TrimExcess()
        {
            this.queue1.TrimExcess();
            this.queue2.TrimExcess();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the remainder of the queue, not to be mistaken for the entire queue.
        /// </summary>
        /// <returns>The enumerator for the remainder of the queue, not to be mistaken for the entire queue.</returns>
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return this.useQueue1 ? this.queue1.GetEnumerator() : this.queue2.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the remainder of the queue, not to be mistaken for the entire queue.
        /// </summary>
        /// <returns>The enumerator for the remainder of the queue, not to be mistaken for the entire queue.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.useQueue1 ? this.queue1.GetEnumerator() : this.queue2.GetEnumerator();
        }

        /// <summary>
        /// Copies the contents of the queue into another array.
        /// </summary>
        /// <param name="array">A one-dimensional array to copy the contents to. This array must use zero-based indexing.</param>
        /// <param name="index">The zero-based index in array at which copying begins.</param>
        void ICollection.CopyTo(Array array, int index)
        {
            ICollection queue;
            if (this.useQueue1)
            {
                queue = this.queue1;
            }
            else
            {
                queue = this.queue2;
            }

            queue.CopyTo(array, index);
            if (this.useQueue1)
            {
                queue = this.queue2;
            }
            else
            {
                queue = this.queue1;
            }

            queue.CopyTo(array, index);
        }
    }
}
