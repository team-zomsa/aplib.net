using System;
using System.Collections.Generic;

namespace Aplib.Core.Tactics
{
    /// <summary>
    /// Represents a tactic that executes the first enabled action from a list of sub-tactics.
    /// </summary>
    public class FirstOfTactic : AnyOfTactic
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FirstOfTactic"/> class with the specified sub-tactics.
        /// </summary>
        /// <param name="subTactics">The list of sub-tactics.</param>
        public FirstOfTactic(params Tactic[] subTactics) : base(subTactics)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FirstOfTactic"/> class with the specified sub-tactics and guard condition.
        /// </summary>
        /// <param name="subTactics">The list of sub-tactics.</param>
        /// <param name="guard">The guard condition.</param>
        public FirstOfTactic(Func<bool> guard, params Tactic[] subTactics) : base(guard, subTactics)
        {
        }

        /// <inheritdoc/>
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
