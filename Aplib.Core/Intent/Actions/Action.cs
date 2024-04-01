using System;

namespace Aplib.Core.Intent.Actions
{
    /// <summary>
    /// Describes an action that can be executed and guarded.
    /// </summary>
    public class Action
    {
        /// <summary>
        /// Gets or sets the effect of the action.
        /// </summary>
        protected System.Action _effect { get; set; }

        /// <summary>
        /// Gets or sets the guard of the action.
        /// </summary>
        protected Func<bool> _guard { get; set; } = () => true;

        /// <summary>
        /// Initializes a new instance of the <see cref="Action{TQuery}" /> class.
        /// </summary>
        /// <param name="effect">The effect of the action.</param>
        public Action(System.Action effect) => _effect = effect;

        /// <summary>
        /// Initializes a new instance of the <see cref="Action{TQuery}" /> class.
        /// </summary>
        /// <param name="effect">The effect of the action.</param>
        /// <param name="guard">The guard of the action.</param>
        public Action(System.Action effect, Func<bool> guard) : this(effect) => _guard = guard;

        /// <summary>
        /// Initializes a new empty instance of the <see cref="Action{TQuery}" /> class.
        /// </summary>
        /// <remarks>Only meant for internal use</remarks>
        protected internal Action()
        {
            _effect = () => { };
            _guard = () => false;
        }

        /// <summary>
        /// Execute the action against the world.
        /// </summary>
        internal virtual void Execute() => _effect();

        /// <summary>
        /// Guard the action against unwanted execution. The result is stored and can be used in the effect.
        /// </summary>
        internal virtual bool IsActionable() => _guard();
    }
}
