using System;

namespace Aplib.Core.Belief
{
    /// <summary>
    /// The <see cref="MemoryBelief{TReference, TObservation}"/> class represents the agent's belief of a single object,
    /// but with additional "memory" of previous observations.
    /// Some <i>object reference</i> is used to generate/update a <i>observation</i> 
    /// (i.e., some piece of information on the game state as perceived by an agent).
    /// This belief also stores a limited amount of previous observations in memory.
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
        /// Initializes a new instance of the <see cref="MemoryBelief{TReference, TObservation}"/> class with an object reference,
        /// and a function to generate/update the observation using the object reference.
        /// Also initializes the memory array with a specified number of slots.
        /// </summary>
        /// <param name="reference">The reference used to generate/update the resource.</param>
        /// <param name="getObservationFromReference">A function that takes a reference and generates/updates a resource.</param>
        /// <param name="framesToRemember">The number of frames to remember back.</param>
        public MemoryBelief(TReference reference, Func<TReference, TObservation> getObservationFromReference, int framesToRemember)
            : base(reference, getObservationFromReference)
        {
            _memorizedObservations = new(framesToRemember);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryBelief{TReference, TObservation}"/> class with an object reference,
        /// a function to generate/update the observation using the object reference,
        /// and a condition on when the observation should be updated.
        /// Also initializes the memory array with a specified number of slots.
        /// </summary>
        /// <param name="reference">The reference used to generate/update the resource.</param>
        /// <param name="getObservationFromReference">A function that takes a reference and generates/updates a resource.</param>
        /// <param name="framesToRemember">The number of frames to remember back.</param>
        /// <param name="shouldUpdate">A function that sets a condition on when the resource should be updated.</param>
        public MemoryBelief(TReference reference, Func<TReference, TObservation> getObservationFromReference, int framesToRemember,
            Func<bool> shouldUpdate)
            : base(reference, getObservationFromReference, shouldUpdate)
        {
            _memorizedObservations = new(framesToRemember);
        }

        /// <summary>
        /// Generates/updates the resource.
        /// Also stores the previous observation in memory.
        /// </summary>
        public override void UpdateBelief()
        {
            // We use the implicit conversion to TObservation to store the observation
            _memorizedObservations.Put(this);
            base.UpdateBelief();
        }

        /// <summary>
        /// Gets the most recently memorized resource.
        /// </summary>
        /// <returns> The most recent memory of the resource.</returns>
        public TObservation GetMostRecentMemory() => _memorizedObservations.GetFirst();

        /// <summary>
        /// Gets the memorized resource at a specific index.
        /// A higher index means a memory further back in time.
        /// If the index is out of bounds, returns the closest element that is in bounds.
        /// </summary>
        /// <returns> The memory of the resource at the specified index.</returns>
        public TObservation GetMemoryAt(int index, bool clamp = false)
        {
            int lastMemoryIndex = _memorizedObservations.Length - 1;
            if (clamp)
                index = Math.Clamp(index, 0, lastMemoryIndex);
            else if (index < 0 || index > lastMemoryIndex)
                throw new IndexOutOfRangeException();
            return _memorizedObservations[index];
        }

        /// <summary>
        /// Gets all the memorized resources.
        /// The first element is the newest memory.
        /// </summary>
        /// <returns> An array of all the memorized resources.</returns>
        public TObservation[] GetAllMemories()
        {
            // For now, we return the entire array, but with empty elements for the unused slots
            return _memorizedObservations.ToArray();

            // TODO: If not all slots are filled, returns a smaller array.
            // Keep track of last non-default index in case of empty slots in the middle
            // int lastNonDefaultIndex = -1;
            // for (int i = 0; i < _memorizedObservations.Length; i++)
            // {
            //     if (!_memorizedObservations[i]!.Equals(default(TObservation)))
            //     {
            //         lastNonDefaultIndex = i;
            //     }
            // }
            // if (lastNonDefaultIndex == -1) return Array.Empty<TObservation>();

            // TObservation[] memories = _memorizedObservations.ToArray(0, lastNonDefaultIndex);
            // return memories;
        }
    }
}