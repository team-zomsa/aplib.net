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
        /// Limited to <see cref="MaxCount"/>.
        /// </summary>
        public int Count
        {
            get => _count;
            private set => _count = value > MaxCount ? MaxCount : value;
        }

        /// <inheritdoc/>
        public bool IsReadOnly => false;

        private int _count;

        private readonly T[] _array;
        private int _head;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExposedQueue{T}"/> class.
        /// </summary>
        /// <param name="size">The size of the array.</param>
        public ExposedQueue(int size)
        {
            MaxCount = size;
            _array = new T[MaxCount];
            _head = MaxCount - 1;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExposedQueue{T}"/> class.
        /// </summary>
        /// <param name="array">An array to use as the circular array.</param>
        public ExposedQueue(T[] array)
        {
            MaxCount = array.Length;
            _array = array;
            _head = MaxCount - 1;
        }

        /// <summary>
        /// Gets the element at the specified index.
        /// </summary>
        /// <param name="index">The index of the element to get.</param>
        /// <returns>The element at the specified index.</returns>
        public T this[int index]
        {
            get => _array[(index + _head + 1) % MaxCount];
            set => _array[(index + _head + 1) % MaxCount] = value;
        }

        /// <summary>
        /// Decrements the head of the array.
        /// </summary>
        private void DecrementHead()
        {
            _head = (_head - 1 + MaxCount) % MaxCount;
        }

        /// <summary>
        /// Puts an element at the start of the array.
        /// </summary>
        /// <param name="value">The element to add to the array</param>
        public void Put(T value)
        {
            _array[_head] = value;
            DecrementHead();
            Count++;
        }

        /// <inheritdoc/>
        public void Add(T item) => Put(item);

        /// <summary>
        /// Gets the element at the head of the array.
        /// </summary>
        /// <returns>The element at the head of the array</returns>
        public T GetHead()
        {
            return _array[_head];
        }

        /// <summary>
        /// Gets the first element of the array.
        /// </summary>
        /// <returns>The last element of the array</returns>
        public T GetFirst()
        {
            return this[0];
        }

        /// <summary>
        /// Copies the circular array to an array.
        /// The head should be the last element of the array.
        /// Copies from start to end inclusive.
        /// </summary>
        /// <param name="array">The array to copy to."</param>
        /// <param name="arrayIndex">The start index of the range to copy.</param>
        /// <param name="endIndex">The end index of the range to copy.</param>
        /// <returns>The circular array as a normal array</returns>
        public void CopyTo(T[] array, int arrayIndex, int endIndex)
        {
            if (arrayIndex < 0 || arrayIndex >= MaxCount)
                throw new System.ArgumentOutOfRangeException(nameof(arrayIndex));
            if (endIndex < 0 || endIndex >= MaxCount)
                throw new System.ArgumentOutOfRangeException(nameof(endIndex));
            if (arrayIndex > endIndex)
                throw new System.ArgumentException("Start index must be less than or equal to end index.");
            if (array.Length < endIndex - arrayIndex + 1)
                throw new System.ArgumentException("Array is too small to copy the range.");

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
        public T[] ToArray(int start = 0, int end = -1)
        {
            end = end == -1 ? Count - 1 : end;
            T[] result = new T[end - start + 1];
            CopyTo(result, start, end);
            return result;
        }

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

        /// <inheritdoc/>
        public bool Remove(T item)
        {
            for (int i = 0; i < Count; i++)
                if (this[i]!.Equals(item))
                {
                    this[i] = default!;
                    return true;
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
    }
}
