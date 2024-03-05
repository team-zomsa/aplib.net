using System;
using System.Collections.Generic;

namespace Aplib.Core.Tactics
{
    /// <summary>
    /// Represents a tactic that executes any of the provided sub-tactics.
    /// </summary>
    public class AnyOfTactic : Tactic
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AnyOfTactic"/> class with the specified sub-tactics.
        /// </summary>
        /// <param name="subTactics">The list of sub-tactics.</param>
        public AnyOfTactic(List<Tactic> subTactics) : base(subTactics)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AnyOfTactic"/> class with the specified sub-tactics and guard condition.
        /// </summary>
        /// <param name="subTactics">The list of sub-tactics.</param>
        /// <param name="guard">The guard condition.</param>
        public AnyOfTactic(List<Tactic> subTactics, Func<bool> guard) : base(subTactics, guard)
        {
        }

        /// <inheritdoc/>
        public override List<PrimitiveTactic> GetFirstEnabledActions()
        {
            List<PrimitiveTactic> primitiveTactics = new();

            foreach (Tactic subTactic in SubTactics)
            {
                primitiveTactics.AddRange(subTactic.GetFirstEnabledActions());
            }

            return primitiveTactics;
        }
    }
}
