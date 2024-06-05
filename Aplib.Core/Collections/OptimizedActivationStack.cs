using System.Collections.Generic;
using System.Linq;

namespace Aplib.Core.Collections
{
    /// <summary>
    /// A stack that has a predefined set of items that can be <i>activated</i>
    /// (i.e., pushed on top of the stack).
    /// When an item that is already on the stack is activated, it is <i>reactivated</i>
    /// (i.e., moved to the top of the stack).
    /// </summary>
    /// <remarks>
    /// The <see cref="OptimizedActivationStack{T}"/> allows for O(1) activation and
    /// reactivation of an arbitrary stack item.
    /// </remarks>
    /// <typeparam name="T">The type of the items that are put on the stack.</typeparam>
    public class OptimizedActivationStack<T>
    {
        /// <summary>
        /// The top item on the stack.
        /// </summary>
        private StackItem? _top;

        /// <summary>
        /// Gets the activatable stack items.
        /// </summary>
        /// <remarks>
        /// The stack items are exposed, since they should be accessible from the outside to
        /// provide O(1) activation of a stack item with <see cref="Activate(StackItem)"/>.
        /// </remarks>
        public IEnumerable<StackItem> ActivatableStackItems { get; }

        private int _count = 0;

        /// <summary>
        /// Gets the number of items that are currently activated (i.e., on the stack).
        /// </summary>
        /// <exception cref="System.InvalidOperationException">
        /// Thrown when the stack count is negative.
        /// </exception>"
        public int Count
        {
            get => _count;
            private set
            {
                if (value < 0) throw new System.InvalidOperationException("The stack count cannot be negative.");

                _count = value;
            }
        }

        /// <summary>
        /// Initializes an optimized activation stack with a set of activatable data.
        /// </summary>
        /// <param name="activatables">A set of activatable items that could be pushed on the stack.</param>
        public OptimizedActivationStack(T[] activatables)
        {
            // Setup the activatable stack items.
            ActivatableStackItems = activatables.Select(activatable => new StackItem(activatable, this));
        }

        /// <summary>
        /// Activates an item (i.e., pushes an item on top of the stack).
        /// If the pushed item is already on the stack,
        /// it is extracted from the stack before it is put on top again.
        /// </summary>
        /// <param name="item">
        /// The stack item that is pushed on top of the stack (i.e., it is activated).
        /// </param>
        /// <exception cref="System.ArgumentException">
        /// Thrown when an item is pushed that belongs to a different stack.
        /// </exception>
        public void Activate(StackItem item)
        {
            if (item.ActivationStack != this)
                throw new System.ArgumentException(
                    "Cannot push an item that is not an activatable of this activation stack."
                );

            // Handle the case when the stack is empty.
            if (Count == 0)
            {
                _top = item;
                item.IsActive = true;
                Count++;
                return;
            }

            // If the item is already on the stack, remove it before adding it on top again.
            // We also don't increment the count in this case, as no new item is added to the stack.
            if (item.IsActive)
                item.RemoveFromStack();
            else
                Count++;

            // Push the new item on top of the stack.
            item.PushOnStackAfter(_top!);
            _top = item;
        }

        /// <summary>
        /// Peeks the top item from the stack.
        /// </summary>
        /// <returns>The top item.</returns>
        /// <exception cref="System.InvalidOperationException">Thrown when the stack is empty.</exception>
        public T Peek()
        {
            if (_top is null) throw new System.InvalidOperationException("The stack is empty.");

            return _top.Data;
        }

        /// <summary>
        /// Pops the top item from the stack.
        /// </summary>
        /// <returns>The popped item.</returns>
        /// <exception cref="System.InvalidOperationException">Thrown when the stack is empty.</exception>
        public T Pop()
        {
            if (_top is null) throw new System.InvalidOperationException("The stack is empty.");

            // Pop the top item from the stack.
            StackItem _oldTop = _top;
            _top = _top.Previous;
            _oldTop.RemoveFromStack();

            Count--;

            return _oldTop.Data;
        }

        /// <summary>
        /// Represents (i.e., encapsulates) an item on the activation stack.
        /// </summary>
        /// <remarks>
        /// This class is public, because the whole stack item should be accessible from the outside to
        /// provide O(1) activation of a stack item with <see cref="Activate(StackItem)"/>.
        /// </remarks>
        public sealed class StackItem
        {
            /// <summary>
            /// Gets the data that this stack item represents.
            /// </summary>
            public T Data { get; }

            /// <summary>
            /// Gets the activation stack instance that this stack item belongs to.
            /// </summary>
            public OptimizedActivationStack<T> ActivationStack { get; }

            /// <summary>
            /// Gets or sets the previous (below) item on the stack.
            /// </summary>
            public StackItem? Previous { get; set; }

            /// <summary>
            /// Gets or sets the next (above) item on the stack.
            /// </summary>
            public StackItem? Next { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether the item is currently on the stack.
            /// </summary>
            public bool IsActive { get; set; } = false;

            /// <summary>
            /// Creates a stack item for the <see cref="OptimizedActivationStack{T}"/> class.
            /// </summary>
            /// <param name="data">The data to put on the stack.</param>
            /// <param name="activationStack">The activation stack instance that this stack item belongs to.</param>
            public StackItem(T data, OptimizedActivationStack<T> activationStack)
            {
                Data = data;
                ActivationStack = activationStack;
            }

            /// <summary>
            /// Links this item before another item.
            /// </summary>
            /// <param name="item">The item that should be on top.</param>
            private void SetNext(StackItem? item)
            {
                Next = item;

                if (item is not null) item.Previous = this;
            }

            /// <summary>
            /// Links this item after another item.
            /// </summary>
            /// <param name="item">The item that should be below.</param>
            private void SetPrevious(StackItem? item)
            {
                Previous = item;

                if (item is not null) item.Next = this;
            }

            /// <summary>
            /// Pushes an item that is not on the stack yet after another item that is already on the stack.
            /// </summary>
            /// <param name="item">An item that is already on the stack.</param>
            /// <exception cref="System.ArgumentException">
            /// Thrown when an item is pushed after an item that is not on the same stack,
            /// when an item is already on the stack,
            /// or when an item is pushed after an item that is not on the stack.
            /// </exception>
            public void PushOnStackAfter(StackItem item)
            {
                if (ActivationStack != item.ActivationStack)
                    throw new System.ArgumentException(
                        "Cannot push an item after an item that is not an activatable of the same stack."
                    );
                if (IsActive) throw new System.ArgumentException("Cannot push an item that is already on the stack.");
                if (!item.IsActive)
                    throw new System.ArgumentException("Cannot push an item after an item that is not on the stack.");

                SetNext(item.Next);
                SetPrevious(item);

                IsActive = true;
            }

            /// <summary>
            /// Safely remove the item from the stack.
            /// </summary>
            public void RemoveFromStack()
            {
                Previous?.SetNext(Next);
                Next?.SetPrevious(Previous);

                Previous = null;
                Next = null;

                IsActive = false;
            }
        }
    }
}
