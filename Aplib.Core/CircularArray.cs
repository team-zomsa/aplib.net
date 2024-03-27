using System;

namespace Aplib.Core
{
    /// <summary>
    /// An array that wraps around when it reaches its end.
    /// Functionally works sort of like a queue with indexing.
    /// </summary>
    public class CircularArray<T>
    {

        /// <summary>
        /// The length of the array.
        /// </summary>
        public int Length { get; private set; }
        private readonly T[] _array;
        private int _head;

        /// <summary>
        /// Initializes a new instance of the <see cref="CircularArray{T}"/> class.
        /// </summary>
        /// <param name="size">The size of the array.</param>
        public CircularArray(int size)
        {
            Length = size;
            _array = new T[Length];
            _head = Length - 1;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CircularArray{T}"/> class.
        /// </summary>
        /// <param name="array">An array to use as the circular array.</param>
        public CircularArray(T[] array)
        {
            Length = array.Length;
            _array = array;
            _head = Length - 1;
        }

        /// <summary>
        /// Gets the element at the specified index.
        /// </summary>
        /// <param name="index">The index of the element to get.</param>
        /// <returns>The element at the specified index.</returns>
        public T this[int index]
        {
            get => _array[(index + _head + 1) % Length];
            set => _array[(index + _head + 1) % Length] = value;
        }

        /// <summary>
        /// Decrements the head of the array.
        /// </summary>
        private void DecrementHead()
        {
            _head = (_head - 1 + Length) % Length;
        }

        /// <summary>
        /// Puts an element at the start of the array.
        /// </summary>
        /// <param name="value">The element to add to the array</param>
        public void Put(T value)
        {
            _array[_head] = value;
            DecrementHead();
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
        /// Gets the first element of the array.
        /// </summary>
        /// <returns>The last element of the array</returns>
        public T GetFirst()
        {
            return this[0];
        }

        /// <summary>
        /// Converts the circular array to an array.
        /// The head should be the last element of the array.
        /// </summary>
        /// <returns>The circular array as a normal array</returns>
        public T[] ToArray()
        {
            T[] result = new T[Length];
            for (int i = 0; i < Length; i++)
            {
                result[i] = this[i];
            }
            return result;
        }
    }
}
