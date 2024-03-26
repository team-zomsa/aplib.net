using System;

namespace Aplib.Core
{
    /// <summary>
    /// An array that wraps around when it reaches its end.
    /// Functionally works as a queue with indexing.
    /// </summary>
    public class CircularArray<T>
    {
        private readonly T[] _array;
        private int _head;

        /// <summary>
        /// Initializes a new instance of the <see cref="CircularArray{T}"/> class.
        /// </summary>
        /// <param name="size">The size of the array.</param>
        public CircularArray(int size)
        {
            _array = new T[size];
            _head = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CircularArray{T}"/> class.
        /// </summary>
        /// <param name="array">An array to use as the circular array.</param>
        public CircularArray(T[] array)
        {
            _array = array;
            _head = 0;
        }

        /// <summary>
        /// Gets the element at the specified index.
        /// </summary>
        /// <param name="index">The index of the element to get.</param>
        /// <returns>The element at the specified index.</returns>
        public T this[int index]
        {
            get => _array[(index + _head) % _array.Length];
            set => _array[(index + _head) % _array.Length] = value;
        }

        /// <summary>
        /// Increments the head of the array.
        /// </summary>
        private void IncrementHead()
        {
            _head = (_head + 1) % _array.Length;
        }

        /// <summary>
        /// Puts an element at the head of the array.
        /// The head is then incremented.
        /// </summary>
        /// <param name="value">The element to add to the array</param>
        public void Put(T value)
        {
            _array[_head] = value;
            IncrementHead();
        }

        /// <summary>
        /// Gets the element at the head of the array.
        /// </summary>
        /// <returns>The element at the head of the array</returns>
        public T GetHead()
        {
            return _array[_head];
        }

        /// <summary>
        /// Gets the last element of the array.
        /// </summary>
        /// <returns>The last element of the array</returns>
        public T GetLast()
        {
            return _array[(_head - 1 + _array.Length) % _array.Length];
        }

        /// <summary>
        /// Converts the circular array to an array.
        /// The head should be the first element of the array.
        /// </summary>
        /// <returns>The circular array as a normal array</returns>
        public T[] ToArray()
        {
            T[] result = new T[_array.Length];
            for (int i = 0; i < _array.Length; i++)
            {
                result[i] = this[i];
            }
            return result;
        }
    }
}
