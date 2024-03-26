using System;
using System.Collections.Generic;
using Aplib.Core;

namespace Aplib.Core.Belief
{
	/// <summary>
    /// The <see cref="MemoryBelief{TReference, TResource}"/> class represents a belief with "memory".
    /// Some <i>object reference</i> is used to generate/update a <i>resource</i> 
    /// (i.e., some piece of information on the game state as perceived by an agent).
    /// 
    /// </summary>
    /// <remarks>
    /// It implements the <see cref="IBelief"/> interface.
    /// It supports implicit conversion to <typeparamref name="TObservation"/>.
    /// </remarks>
    /// <typeparam name="TReference">The type of the reference used to generate/update the resource.</typeparam>
    /// <typeparam name="TObservation">The type of the resource the belief represents.</typeparam>
	public class MemoryBelief<TReference, TObservation> : Belief<TReference, TObservation>
	{
        /// <summary>
        /// A "memorized" resouce, from the last time the belief was updated.
        /// </summary>
        private readonly CircularArray<TObservation> _memorizedObservations;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryBelief{TReference, TResource}"/>
        /// </summary>
        /// <param name="reference">The reference used to generate/update the resource.</param>
        /// <param name="getResourceFromReference">A function that takes a reference and generates/updates a resource.</param>
        /// <param name="framesToRemember">The number of frames to remember back.</param>
        public MemoryBelief(TReference reference, Func<TReference, TObservation> getResourceFromReference, int framesToRemember)
            : base(reference, getResourceFromReference, () => true)
        {
            _memorizedObservations = new(framesToRemember);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryBelief{TReference, TResource}"/>
        /// </summary>
        /// <param name="reference">The reference used to generate/update the resource.</param>
        /// <param name="getResourceFromReference">A function that takes a reference and generates/updates a resource.</param>
        /// <param name="framesToRemember">The number of frames to remember back.</param>
        /// <param name="shouldUpdate">A function that sets a condition on when the resource should be updated.</param>
        public MemoryBelief(TReference reference, Func<TReference, TObservation> getResourceFromReference, int framesToRemember, 
            Func<bool> shouldUpdate)
            : base(reference, getResourceFromReference, shouldUpdate)
        {
            _memorizedObservations = new(framesToRemember);
        }

        /// <summary>
        /// Generates/updates the resource if the shouldUpdate condition is satisfied.
        /// Also stores the previous observation in memory.
        /// </summary>
        public override void UpdateBelief()
        {
            // We use the implicit conversion to TObservation to store the observation
            if (_shouldUpdate()) _memorizedObservations.Put(this);
            base.UpdateBelief();
        }

        /// <summary>
        /// Gets the most recently memorized resource
        /// </summary>
        /// <returns> The most recent memory of the resource.</returns>
        public TObservation GetMostRecentMemory() => _memorizedObservations.GetLast();

        /// <summary>
        /// Gets the memorized resource at a specific index
        /// </summary>
        /// <returns> The memory of the resource at the specified index.</returns>
        public TObservation GetMemoryAt(int index) => _memorizedObservations[index];

        /// <summary>
        /// Gets all the memorized resources
        /// </summary>
        /// <returns> A list of all the memorized resources.</returns>
        public TObservation[] GetAllMemories() => _memorizedObservations.ToArray();
    }
}