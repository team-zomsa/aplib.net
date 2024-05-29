using System;
using System.Collections;
using System.Collections.Generic;

namespace Aplib.Core.Collections
{
    /// <summary>
    /// A queue with all elements exposed.
    /// Functionally works like a queue with indexing.
    /// It has a MaxCount and Count. MaxCount being the maximal length of the queue, 
    /// and Count being the actual number of elements in the queue.
    /// </summary>
    /// <remarks>
    /// When adding an element to a full queue, all other elements are shifted one place like so:
    /// [4, 3, 2, 1], Put(5) => [5, 4, 3, 2]
    /// </remarks>
    public class ExposedQueue<T> : ICollection<T>
    {
        /// <summary>
        /// The length of the array.
        /// </summary>
        public int MaxCount { get; private set; }

        /// <summary>
        /// Actual number of elements in the array.
        /// </summary>
        public int Count { get; private set; }

        /// <inheritdoc/>
        public bool IsReadOnly => false;

        private readonly T[] _array;
        private int _head;

        /// <summary>
        /// Initializes a new empty instance of the <see cref="ExposedQueue{T}"/> class.
        /// </summary>
        /// <param name="size">The maximum size of the queue.</param>
        public ExposedQueue(int size)
        {
            MaxCount = size;
            Count = 0;
            _array = new T[MaxCount];
            _head = MaxCount - 1;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExposedQueue{T}"/> class
        /// with an array to use as basis for the queue.
        /// By default, assumes the array is filled. 
        /// </summary>
        /// <param name="array">An array to use as the circular array.</param>
        /// <param name="count">The number of actual elements in the array.</param>
        /// <remarks>
        /// The MaxCount of the queue will be set to the length of the array.
        /// If the array is not fully filled, the Count should be specified.
        /// </remarks>
        public ExposedQueue(T[] array, int count)
        {
            if (count > array.Length)
                throw new ArgumentOutOfRangeException(nameof(count), "Count cannot exceed the length of the array.");
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), "Count cannot be negative.");

            MaxCount = array.Length;
            _array = array;
            _head = MaxCount - 1;
            Count = count;
        }

        /// <inheritdoc cref="ExposedQueue{T}(T[],int)"/>
        public ExposedQueue(T[] array)
            : this(array, array.Length)
        {
        }

        /// <summary>
        /// Gets the element at the specified index. Throws an exception if the index is out of bounds.
        /// </summary>
        /// <param name="index">The index of the element to get.</param>
        /// <returns>The element at the specified index.</returns>
        /// <exception cref="ArgumentOutOfRangeException">  
        /// Thrown when the index is out of range.  
        /// </exception> 
        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= Count)
                    throw new ArgumentOutOfRangeException(nameof(index));
                return _array[(index + _head + 1) % MaxCount];
            }
            private set
            {
                if (index < 0 || index >= Count)
                    throw new ArgumentOutOfRangeException(nameof(index));
                _array[(index + _head + 1) % MaxCount] = value;
            }
        }

        /// <summary>
        /// Puts an element at the start of the queue.
        /// </summary>
        /// <param name="value">The element to add to the queue.</param>
        public void Put(T value)
        {
            _array[_head] = value;
            DecrementHead();
            if (Count < MaxCount)
                Count++;
        }

        /// <inheritdoc/>
        public void Add(T item) => Put(item);

        /// <summary>
        /// Gets the element at the end of the queue.
        /// </summary>
        /// <returns>The element at the end of the queue.</returns>
        public T GetLast() => _array[_head];

        /// <summary>
        /// Gets the first element of the queue.
        /// </summary>
        /// <returns>The first element of the queue.</returns>
        public T GetFirst() => this[0];

        /// <summary>
        /// Copies the ExposedQueue to an array.
        /// The head should be the last element of the array.
        /// Copies from start to end inclusive.
        /// </summary>
        /// <param name="array">The array to copy to."</param>
        /// <param name="arrayIndex">The start index of the range to copy.</param>
        /// <param name="endIndex">The end index of the range to copy.</param>
        /// <returns>The ExposedQueue as a regular array.</returns>
        public void CopyTo(T[] array, int arrayIndex, int endIndex)
        {
            if (arrayIndex < 0 || arrayIndex >= Count)
                throw new ArgumentOutOfRangeException(nameof(arrayIndex));
            if (endIndex < 0 || endIndex >= Count)
                throw new ArgumentOutOfRangeException(nameof(endIndex));
            if (arrayIndex > endIndex)
                throw new ArgumentException("Start index must be less than or equal to end index.");

            for (int i = 0; i < endIndex - arrayIndex + 1; i++)
                array[i] = this[arrayIndex + i];
        }

        /// <inheritdoc/>
        public void CopyTo(T[] array, int arrayIndex) => CopyTo(array, arrayIndex, arrayIndex + Count - 1);

        /// <summary>
        /// Converts the ExposedQueue to an array.
        /// </summary>
        /// <param name="start">The start index of the range to convert.</param>
        /// <param name="end">The end index of the range to convert.</param>
        /// <returns>An array containing the elements within the specified range.</returns>
        public T[] ToArray(int start, int end)
        {
            if (start < 0 || start >= Count)
                throw new ArgumentOutOfRangeException(nameof(start), "Start index must be within the bounds of the array.");
            if (end < 0 || end >= Count)
                throw new ArgumentOutOfRangeException(nameof(end), "End index must be within the bounds of the array.");
            T[] result = new T[end - start + 1];
            CopyTo(result, start, end);
            return result;
        }

        /// <summary>
        /// Converts the ExposedQueue to an array. Only returns the used slots.
        /// </summary>
        /// <returns>An array containing the elements within the specified range.</returns>
        public T[] ToArray() => ToArray(0, Count - 1);

        /// <inheritdoc/>
        public void Clear()
        {
            for (int i = 0; i < MaxCount; i++)
                _array[i] = default!;
            _head = MaxCount - 1;
            Count = 0;
        }

        /// <inheritdoc/>
        public bool Contains(T item)
        {
            for (int i = 0; i < Count; i++)
                if (this[i]!.Equals(item))
                    return true;
            return false;
        }

        /// <summary>
        /// Removes the specified item from the queue and shifts remaining elements to the left.
        /// For example, given the queue [4, 3, 2, 1], if you call Remove(3), the resulting queue will be [4, 2, 1].
        /// </summary>
        /// <param name="item">The item to remove.</param>
        /// <returns>True if the item was successfully removed; otherwise, false.</returns>
        /// <remarks>
        /// The MaxCount will not change, but the Count will decrease by one.
        /// </remarks>
        public bool Remove(T item)
        {
            for (int i = 0; i < Count; i++)
            {
                if (this[i]!.Equals(item))
                {
                    RemoveAt(i);
                    return true;
                }
            }

            return false;
        }

        /// <inheritdoc/>
        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
                yield return this[i];
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Decrements the head of the array.
        /// </summary>
        private void DecrementHead() => _head = (_head - 1 + MaxCount) % MaxCount;

        /// <summary>
        /// Removes the element at the specified index.
        /// Shifts all other elements to the left.
        /// </summary>
        /// <param name="index">The index of the element to remove.</param>
        private void RemoveAt(int index)
        {
            for (int i = index; i < Count - 1; i++)
                this[i] = this[i + 1];
            Count--;
        }
    }
}
