using System;

namespace Aplib.Core
{
    /// <summary>
    /// Describes an action that can be executed and guarded.
    /// </summary>
    public class Action
    {
        /// <summary>
        /// Gets or sets the effect of the action.
        /// </summary>
        protected System.Action Effect { get; set; }

        /// <summary>
        /// Gets or sets the guard of the action.
        /// </summary>
        protected Func<bool> Guard { get; set; } = () => true;

        /// <summary>
        /// Initializes a new empty instance of the <see cref="Action{TQuery}"/> class.
        /// </summary>
        /// <remarks>Only meant for internal use</remarks>
        protected internal Action()
        {
            Effect = () => { };
            Guard = () => false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Action{TQuery}"/> class.
        /// </summary>
        /// <param name="effect">The effect of the action.</param>
        public Action(System.Action effect) => Effect = effect;

        /// <summary>
        /// Initializes a new instance of the <see cref="Action{TQuery}"/> class.
        /// </summary>
        /// <param name="effect">The effect of the action.</param>
        /// <param name="guard">The guard of the action.</param>
        public Action(System.Action effect, Func<bool> guard) : this(effect) => Guard = guard;

        /// <summary>
        /// Execute the action against the world.
        /// </summary>
        public virtual void Execute() => Effect();

        /// <summary>
        /// Guard the action against unwanted execution. The result is stored and can be used in the effect.
        /// </summary>
        public virtual bool IsActionable() => Guard();
    }
}
