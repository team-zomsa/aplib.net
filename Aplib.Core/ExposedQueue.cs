using System;
using System.Collections;
using System.Collections.Generic;

namespace Aplib.Core
{
    /// <summary>
    /// A queue with all elements exposed.
    /// Functionally works like a queue with indexing.
    /// </summary>
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
        /// Initializes a new instance of the <see cref="ExposedQueue{T}"/> class.
        /// </summary>
        /// <param name="size">The size of the array.</param>
        public ExposedQueue(int size)
        {
            MaxCount = size;
            Count = 0;
            _array = new T[MaxCount];
            _head = MaxCount - 1;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExposedQueue{T}"/> class.
        /// By default, assumes the array is filled.
        /// </summary>
        /// <param name="array">An array to use as the circular array.</param>
        /// <param name="count">The number of actual elements in the array.</param>
        public ExposedQueue(T[] array, int? count = null)
        {
            if (count > array.Length)
                throw new ArgumentOutOfRangeException(nameof(count), "Count cannot exceed the length of the array.");
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), "Count cannot be negative.");

            MaxCount = array.Length;
            _array = array;
            _head = MaxCount - 1;
            Count = count ?? MaxCount;
        }

        /// <summary>
        /// Gets the element at the specified index. Throws an exception if the index is out of bounds.
        /// </summary>
        /// <param name="index">The index of the element to get.</param>
        /// <returns>The element at the specified index.</returns>
        public T this[int index]
        {
            get 
            {
                if (index < 0 || index >= Count)
                    throw new ArgumentOutOfRangeException(nameof(index));
                return _array[(index + _head + 1) % MaxCount];
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
        /// DOES NOT DO ANYTHING.
        /// </summary>
        /// <returns>False.</returns>
        /// <remarks>
        /// Method is there to comply with the ICollection interface. 
        /// Does not actually do anything since this is a queue, and queues do not allow removal of specific elements.
        /// </remarks>
        public bool Remove(T item) => false;

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
    }
}
