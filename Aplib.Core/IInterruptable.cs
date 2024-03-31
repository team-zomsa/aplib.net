using Aplib.Core.Belief;
using Aplib.Core.Desire;
using System;

namespace Aplib.Core
{
    /// <summary>
    /// Defines a class that can be interrupted and reinstated.
    /// </summary>
    public interface IInterruptable<TBeliefSet>
        where TBeliefSet : IBeliefSet
    {
        /// <summary>
        /// Interrupts the current goal structure.
        /// </summary>
        /// <param name="beliefSet">The beliefset at the time of interrupting.</param>
        public void Interrupt(TBeliefSet beliefSet);

        /// <summary>
        /// Reinstates the goal structure after an interrupt.
        /// </summary>
        /// <param name="beliefSet">The beliefset at the time of reinstating.</param>
        public void Reinstate(TBeliefSet beliefSet);


        /// <summary>
        /// Handles the interrupt event.
        /// </summary>
        public event EventHandler<InterruptableEventArgs<TBeliefSet>> OnInterrupt;

        /// <summary>
        /// Handles the reinstate event.
        /// </summary>
        public event EventHandler<InterruptableEventArgs<TBeliefSet>> OnReinstate;
    }
}
