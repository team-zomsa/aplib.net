using Aplib.Core.Belief;
using System;

namespace Aplib.Core.Desire
{
    public class InterruptableEventArgs<TBeliefSet> : EventArgs
        where TBeliefSet : IBeliefSet
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BeliefSetEventArgs" /> class.
        /// </summary>
        /// <param name="beliefSet">The beliefset of the agent.</param>
        public InterruptableEventArgs(TBeliefSet beliefSet) => BeliefSet = beliefSet;

        /// <summary>
        /// Gets the beliefset of the agent.
        /// </summary>
        public TBeliefSet BeliefSet { get; }
    }
}
