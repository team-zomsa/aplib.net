using System;

namespace Aplib.Core.Intent.Actions
{
    /// <summary>
    /// Describes an action that can be executed and guarded.
    /// </summary>
    public class Action
    {
        /// <summary>
        ///     Data about the Action such as a name and description, this may be useful for debugging or logging.
        /// </summary>
        public Metadata Metadata { get; }
        
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
        /// <param name="name">The name of this Action, used to quickly display this goal in several contexts.</param>
        /// <param name="description">
        /// The description of this Action, used to explain this goal in several contexts.
        /// </param>
        protected internal Action(string name, string? description = null)
        {
            Effect = () => { };
            Guard = () => false;
            Metadata = new Metadata(name, description);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Action{TQuery}"/> class.
        /// </summary>
        /// <param name="effect">The effect of the action.</param>
        /// <param name="name">The name of this Action, used to quickly display this goal in several contexts.</param>
        /// <param name="description">
        /// The description of this Action, used to explain this goal in several contexts.
        /// </param>
        public Action(System.Action effect, string name, string? description = null)
        {
            Effect = effect;
            Metadata = new Metadata(name, description);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Action{TQuery}"/> class.
        /// </summary>
        /// <param name="effect">The effect of the action.</param>
        /// <param name="guard">The guard of the action.</param>
        /// <param name="name">The name of this Action, used to quickly display this goal in several contexts.</param>
        /// <param name="description">
        /// The description of this Action, used to explain this goal in several contexts.
        /// </param>
        public Action(System.Action effect, Func<bool> guard, string name, string? description = null)
            : this(effect, name, description) => Guard = guard;

        /// <summary>
        /// Execute the action against the world.
        /// </summary>
        internal virtual void Execute() => Effect();

        /// <summary>
        /// Guard the action against unwanted execution. The result is stored and can be used in the effect.
        /// </summary>
        internal virtual bool IsActionable() => Guard();
    }
}
