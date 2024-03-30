using Aplib.Core.Belief;
using System;

namespace Aplib.Core.Desire
{
    /// <summary>
    /// Describes the parameters of an interruptable event.
    /// </summary>
    /// <typeparam name="TBeliefSet">The beliefset of the agent.</typeparam>
    public class InterruptableEventArgs<TBeliefSet> : EventArgs
        where TBeliefSet : IBeliefSet
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InterruptableEventArgs{TBeliefSet}" /> class.
        /// </summary>
        /// <param name="beliefSet">The beliefset of the agent.</param>
        public InterruptableEventArgs(TBeliefSet beliefSet) => BeliefSet = beliefSet;

        /// <summary>
        /// Gets the beliefset of the agent.
        /// </summary>
        public TBeliefSet BeliefSet { get; }
    }
}
