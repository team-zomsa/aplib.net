using System;
using System.Collections.Generic;

namespace Aplib.Core.Belief
{
	/// <summary>
    /// The <see cref="MultipleMemoryBelief{TReference, TResource}"/> class represents a belief with "memory".
    /// Some <i>object reference</i> is used to generate/update a <i>resource</i> 
    /// (i.e., some piece of information on the game state as perceived by an agent).
    /// 
    /// </summary>
    /// <remarks>
    /// It implements the <see cref="IBelief"/> interface.
    /// It supports implicit conversion to <typeparamref name="TResource"/>.
    /// </remarks>
    /// <typeparam name="TReference">The type of the reference used to generate/update the resource.</typeparam>
    /// <typeparam name="TResource">The type of the resource the belief represents.</typeparam>
	public class MultipleMemoryBelief<TReference, TResource> : Belief<TReference, TResource>
	{
        /// <summary>
        /// A "memorized" resouce, from the last time the belief was updated.
        /// TODO:: Create a circular array to store the last n frames, so you can also access an index i.
        /// </summary>
        private List<TResource> _memorizedResource;

        /// <summary>
        /// The number of frames to hold in memory.
        /// </summary>
        private int _framesToRemember;

        /// <summary>
        /// Initializes a new instance of the <see cref="MultipleMemoryBelief{TReference, TResource}"/>
        /// </summary>
        /// <param name="reference">The reference used to generate/update the resource.</param>
        /// <param name="getResourceFromReference">A function that takes a reference and generates/updates a resource.</param>
        /// <param name="framesToRemember">The number of frames to remember back.</param>
        public MultipleMemoryBelief(TReference reference, Func<TReference, TResource> getResourceFromReference, int framesToRemember)
            : base(reference, getResourceFromReference, () => true)
        {
            _memorizedResource = new(framesToRemember);
            _framesToRemember = framesToRemember;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultipleMemoryBelief{TReference, TResource}"/>
        /// </summary>
        /// <param name="reference">The reference used to generate/update the resource.</param>
        /// <param name="getResourceFromReference">A function that takes a reference and generates/updates a resource.</param>
        /// <param name="framesToRemember">The number of frames to remember back.</param>
        /// <param name="updateIf">A function that sets a condition on when the resource should be updated.</param>
        public MultipleMemoryBelief(TReference reference, Func<TReference, TResource> getResourceFromReference, int framesToRemember, 
            Func<bool> updateIf)
            : base(reference, getResourceFromReference, updateIf)
        {
            _memorizedResource = new(framesToRemember);
        }

        /// <summary>
        /// Generates/updates the resource if the updateIf condition is satisfied.
        /// The resource is then updated by calling the getResourceFromReference function.
        /// </summary>
        public new void UpdateBelief()
        {
            base.UpdateBelief();
            // if (_updateIf())
            // {
            //     _memorizedResource = _resource;
            // }
        }

        /// <summary>
        /// Gets the most recently memorized resource
        /// </summary>
        /// <returns> The most recent memory of the resource.</returns>
        public TResource GetMostRecentMemory() => _memorizedResource[^1];

        /// <summary>
        /// Gets the memorized resource at a specific index
        /// </summary>
        /// <returns> The memory of the resource at the specified index.</returns>
        public TResource GetMemoryAt(int index) => _memorizedResource[index];

        /// <summary>
        /// Gets all the memorized resources
        /// </summary>
        /// <returns> A list of all the memorized resources.</returns>
        public List<TResource> GetAllMemories() => _memorizedResource;
    }
}