﻿using System;
using Action = Aplib.Core.Intent.Actions.Action;

namespace Aplib.Core.Intent.Tactics
{
    /// <summary>
    /// Represents a tactic that executes the first enabled action from a list of sub-tactics.
    /// </summary>
    public class FirstOfTactic : AnyOfTactic
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FirstOfTactic"/> class with the specified sub-tactics.
        /// </summary>
        /// <param name="metadata">
        /// Metadata about this tactic, used to quickly display the tactic in several contexts.
        /// </param>
        /// <param name="subTactics">The list of sub-tactics.</param>
        public FirstOfTactic(Metadata? metadata = null, params Tactic[] subTactics)
            : base(metadata, subTactics)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FirstOfTactic"/> class with the specified sub-tactics and guard condition.
        /// </summary>
        /// <param name="guard">The guard condition.</param>
        /// <param name="metadata">
        /// Metadata about this tactic, used to quickly display the tactic in several contexts.
        /// </param>
        /// <param name="subTactics">The list of sub-tactics.</param>
        public FirstOfTactic(Func<bool> guard, Metadata? metadata = null, params Tactic[] subTactics)
            : base(guard, metadata, subTactics)
        {
        }

        /// <inheritdoc />
        public override Action? GetAction()
        {
            foreach (Tactic subTactic in SubTactics)
            {
                Action? action = subTactic.GetAction();

                if (action is not null)
                    return action;
            }

            return null;
        }
    }
}
