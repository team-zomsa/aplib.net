using System;

namespace Aplib.Core.Intent.Actions
{
    /// <summary>
    /// Describes an action that can be executed and guarded with a query that stores the result of the guard.
    /// The result can be used in the effect.
    /// </summary>
    /// <typeparam name="TQuery">The type of the query of the action</typeparam>
    public class GuardedAction<TQuery> : Action
    {
        /// <summary>
        /// Gets or sets the result of the guard.
        /// </summary>
        protected TQuery? StoredGuardResult { get; set; }

        /// <summary>
        /// Gets or sets the effect of the action.
        /// </summary>
        protected new System.Action<TQuery> Effect { get; set; }

        /// <summary>
        /// Gets or sets the guard of the action.
        /// </summary>
        protected new Func<TQuery?> Guard { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GuardedAction{TQuery}"/> class.
        /// </summary>
        /// <param name="effect">The effect of the action.</param>
        /// <param name="guard">The guard of the action.</param>
        /// <param name="metadata">
        /// Metadata about this action, used to quickly display the action in several contexts.
        /// </param>
        public GuardedAction(Action<TQuery> effect, Func<TQuery?> guard, Metadata? metadata = null)
            : base(metadata)
        {
            Effect = effect;
            Guard = guard;
        }

        /// <inheritdoc/>
        internal override void Execute() => Effect(StoredGuardResult!);

        /// <inheritdoc/>
        internal override bool IsActionable()
        {
            StoredGuardResult = Guard();
            return StoredGuardResult is not null;
        }
    }
}
