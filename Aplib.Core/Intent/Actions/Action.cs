using Aplib.Core.Belief;
using System;

namespace Aplib.Core.Intent.Actions
{
    /// <summary>
    /// Describes an action that can be executed and guarded.
    /// </summary>
    /// <typeparam name="TBeliefSet">The belief set of the agent.</typeparam>
    public class Action<TBeliefSet> : IAction<TBeliefSet> where TBeliefSet : IBeliefSet
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
        protected System.Action<TBeliefSet> _effect { get; set; }

        /// <summary>
        /// Gets or sets the guard of the action.
        /// </summary>
        protected Func<TBeliefSet, bool> _guard { get; set; } = _ => true;

        /// <summary>
        /// Initializes a new instance of the <see cref="Action{TQuery}" /> class.
        /// </summary>=
        /// <param name="effect">The effect of the action.</param>
        /// <param name="metadata">
        /// Metadata about this action, used to quickly display the action in several contexts.
        /// </param>
        public Action(System.Action<TBeliefSet> effect, Metadata? metadata = null)
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
        public Action(System.Action<TBeliefSet> effect, Func<TBeliefSet, bool> guard, Metadata? metadata = null) : this(effect, metadata) => _guard = guard;

        /// <summary>
        /// Initializes a new empty instance of the <see cref="Action{TQuery}" /> class.
        /// </summary>
        /// <remarks>Only meant for internal use</remarks>
        /// <param name="metadata">
        /// Metadata about this action, used to quickly display the action in several contexts.
        /// </param>
        protected Action(Metadata? metadata)
        {
            _effect = _ => { };
            _guard = _ => false;
            Metadata = metadata ?? new Metadata();
        }

        /// <inheritdoc/>
        public virtual void Execute(TBeliefSet beliefSet) => _effect(beliefSet);


        /// <inheritdoc/>
        public virtual bool IsActionable(TBeliefSet beliefSet) => _guard(beliefSet);
    }
}
