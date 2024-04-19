using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aplib.Core.DataStructures
{
    internal class OptimizedActivationStack<T>
    {
        private ActivationStackItem? _top { get; set; }

        public IEnumerable<ActivationStackItem> ActivatableStackItems { get; }

        public int Count { get; private set; } = 0;

        public OptimizedActivationStack(T[] activatables)
        {
            // Setup the activatable stack items.
            ActivatableStackItems = activatables.Select(activatable => new ActivationStackItem(activatable, this));
        }

        public void Push(ActivationStackItem item)
        {
            if (item.ActivationStack != this)
                throw new ArgumentException("Cannot push an item that is not on this stack.");

            // Handle the case when the stack is empty.
            if (_top is null)
            {
                _top = item;
                item.IsActive = true;
                Count++;
                return;
            }

            // If the item is already on the stack, remove it before adding it on top again.
            // We also don't increment the count in this case, as no new item is added to the stack.
            if (item.IsActive) item.RemoveFromStack();
            else Count++;

            // Push the new item on top of the stack.
            _top.PushAfter(item);
        }

        public T Peek()
        {
            if (_top is null)
                throw new InvalidOperationException("The stack is empty.");

            return _top.Item;
        }

        public T Pop()
        {
            if (_top is null)
                throw new InvalidOperationException("The stack is empty.");

            // Pop the top item from the stack.
            ActivationStackItem _oldTop = _top;
            _top = _top.Previous;
            _oldTop.RemoveFromStack();

            Count--;

            return _oldTop.Item;
        }

        internal class ActivationStackItem
        {
            public T Item { get; }
            public OptimizedActivationStack<T> ActivationStack { get; }
            public ActivationStackItem? Previous { get; set; }
            public ActivationStackItem? Next { get; set; }
            public bool IsActive { get; set; }

            public ActivationStackItem(T item, OptimizedActivationStack<T> activationStack, bool isActive = false)
            {
                Item = item;
                ActivationStack = activationStack;
                IsActive = isActive;
            }

            private void SetNext(ActivationStackItem? item)
            {
                Next = item;

                if (item is not null) item.Previous = this;
            }

            private void SetPrevious(ActivationStackItem? item)
            {
                Previous = item;

                if (item is not null) item.Next = this;
            }

            /// <summary>
            /// Pushes an item that is not on the stack yet after another item that is already on the stack.
            /// </summary>
            /// <param name="item">An item that is already on the stack.</param>
            public void PushAfter(ActivationStackItem item)
            {
                if (ActivationStack != item.ActivationStack)
                    throw new ArgumentException("Cannot push an item after an item that is not on the same stack.");
                if (IsActive)
                    throw new ArgumentException("Cannot push an item that is already on the stack. or is not active.");
                if (!item.IsActive)
                    throw new ArgumentException("Cannot push an item after an item that is not on the stack.");

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
