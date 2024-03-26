using System;
using System.Collections.Generic;

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
        /// TODO: Create a circular array to store the last n frames, so you can also access an index i.
        /// </summary>
        private List<TObservation> _memorizedObservations;

        /// <summary>
        /// The number of frames to hold in memory.
        /// </summary>
        private int _framesToRemember;

        // TODO: Remove once belief is up to date and merged
        private readonly Func<bool> _shouldUpdate = () => true;

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
            _framesToRemember = framesToRemember;

            _shouldUpdate = () => true;
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

            _shouldUpdate = shouldUpdate;
        }

        /// <summary>
        /// Generates/updates the resource if the updateIf condition is satisfied.
        /// The resource is then updated by calling the getResourceFromReference function.
        /// </summary>
        public new void UpdateBelief()
        {
            if (_shouldUpdate())
            {
                // TODO: This should be a circular array, 
                // should be something like "put"
                // We use the implicit conversion to TObservation to store the observation
                _memorizedObservations.Add(this);
            }
            base.UpdateBelief();
        }

        /// <summary>
        /// Gets the most recently memorized resource
        /// </summary>
        /// <returns> The most recent memory of the resource.</returns>
        public TObservation GetMostRecentMemory() => _memorizedObservations[^1];

        /// <summary>
        /// Gets the memorized resource at a specific index
        /// </summary>
        /// <returns> The memory of the resource at the specified index.</returns>
        public TObservation GetMemoryAt(int index) => _memorizedObservations[index];

        /// <summary>
        /// Gets all the memorized resources
        /// </summary>
        /// <returns> A list of all the memorized resources.</returns>
        public List<TObservation> GetAllMemories() => _memorizedObservations;
    }
}