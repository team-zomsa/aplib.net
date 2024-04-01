using System;

namespace Aplib.Core.Intent.Actions
{
    /// <summary>
    /// Describes an action that can be executed and guarded.
    /// </summary>
    public class Action
    {
        /// <summary>
        /// Gets the metadata of the action.
        /// </summary>
        /// <remark>
        /// This metadata may be useful for debugging or logging.
        /// </remark>
        public Metadata Metadata { get; }
        
        /// <summary>
        /// Gets or sets the effect of the action.
        /// </summary>
        protected System.Action _effect { get; set; }

        /// <summary>
        /// Gets or sets the guard of the action.
        /// </summary>
        protected Func<bool> _guard { get; set; } = () => true;

        /// <summary>
        /// Parameterless constructor for internal use.
        /// </summary>
        internal Action() : this(null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Action{TQuery}" /> class.
        /// </summary>=
        /// <param name="effect">The effect of the action.</param>
        /// <param name="metadata">
        /// Metadata about this action, used to quickly display the action in several contexts.
        /// </param>
        public Action(System.Action effect, Metadata? metadata = null)
        {
            _effect = effect;
            Metadata = metadata ?? new Metadata();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Action{TQuery}" /> class.
        /// </summary>
        /// <param name="effect">The effect of the action.</param>
        /// <param name="guard">The guard of the action.</param>
        /// <param name="metadata">
        /// Metadata about this action, used to quickly display the action in several contexts.
        /// </param>
        public Action(System.Action effect, Func<bool> guard, Metadata? metadata = null) : this(effect, metadata) => _guard = guard;

        /// <summary>
        /// Initializes a new empty instance of the <see cref="Action{TQuery}" /> class.
        /// </summary>
        /// <remarks>Only meant for internal use</remarks>
        /// <param name="metadata">
        /// Metadata about this action, used to quickly display the action in several contexts.
        /// </param>
        protected internal Action(Metadata? metadata)
        {
            _effect = () => { };
            _guard = () => false;
            Metadata = metadata ?? new Metadata();
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
