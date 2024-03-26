using System;
using System.Collections.Generic;
using Action = Aplib.Core.Intent.Actions.Action;

namespace Aplib.Core.Intent.Tactics
{
    /// <summary>
    /// Represents a tactic that executes any of the provided sub-tactics.
    /// </summary>
    public class AnyOfTactic : Tactic
    {
        /// <summary>
        /// Gets or sets the sub-tactics of the tactic.
        /// </summary>
        protected LinkedList<Tactic> SubTactics { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AnyOfTactic"/> class with the specified sub-tactics.
        /// </summary>
        /// <param name="subTactics">The list of sub-tactics.</param>
        public AnyOfTactic(params Tactic[] subTactics)
        {
            SubTactics = new();

            foreach (Tactic tactic in subTactics)
            {
                _ = SubTactics.AddLast(tactic);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AnyOfTactic"/> class with the specified sub-tactics and guard condition.
        /// </summary>
        /// <param name="subTactics">The list of sub-tactics.</param>
        /// <param name="guard">The guard condition.</param>
        public AnyOfTactic(Func<bool> guard, params Tactic[] subTactics) : this(subTactics) => Guard = guard;

        /// <inheritdoc/>
        public override Action? GetAction()
        {
            List<Action> actions = new();

            foreach (Tactic subTactic in SubTactics)
            {
                Action? action = subTactic.GetAction();

                if (action is not null)
                    actions.Add(action);
            }

            if (actions.Count == 0)
                return null;

            return actions[ThreadSafeRandom.Next(actions.Count)];
        }
    }
}
