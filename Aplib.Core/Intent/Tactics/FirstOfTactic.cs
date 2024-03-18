using System;
using System.Collections.Generic;

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
        /// <param name="subTactics">The list of sub-tactics.</param>
        public FirstOfTactic(List<Tactic> subTactics) : base(subTactics)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FirstOfTactic"/> class with the specified sub-tactics and guard condition.
        /// </summary>
        /// <param name="subTactics">The list of sub-tactics.</param>
        /// <param name="guard">The guard condition.</param>
        public FirstOfTactic(List<Tactic> subTactics, Func<bool> guard) : base(subTactics, guard)
        {
        }

        /// <inheritdoc/>
        public override List<PrimitiveTactic> GetFirstEnabledTactics()
        {
            foreach (Tactic subTactic in SubTactics)
            {
                List<PrimitiveTactic> firstOfTactics = subTactic.GetFirstEnabledTactics();

                if (firstOfTactics.Count > 0)
                    return firstOfTactics;
            }

            return new();
        }
    }
}
