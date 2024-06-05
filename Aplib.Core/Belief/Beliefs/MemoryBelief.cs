using Aplib.Core.Collections;
using System;

namespace Aplib.Core.Belief.Beliefs
{
    /// <summary>
    /// The <see cref="MemoryBelief{TReference, TObservation}"/> class represents the agent's belief of a single object,
    /// but with additional "memory" of previous observations.
    /// Some <i>object reference</i> is used to generate/update an <i>observation</i>
    /// (i.e., some piece of information on the game state as perceived by an agent).
    /// This belief also stores a limited amount of previous observations in memory.
    /// </summary>
    /// <remarks>
    /// It supports implicit conversion to <typeparamref name="TObservation"/>.
    /// </remarks>
    /// <typeparam name="TReference">
    /// The type of the reference used to generate/update the observation. This <i>must</i> be a reference type, be aware that
    /// this is not enforced by C# if <typeparamref name="TReference"/> is an interface.
    /// </typeparam>
    /// <typeparam name="TObservation">The type of the observation the belief represents.</typeparam>
    public class MemoryBelief<TReference, TObservation> : Belief<TReference, TObservation> where TReference : class
    {
        /// <summary>
        /// A "memorized" resource, from the last time the belief was updated.
        /// </summary>
        protected readonly ExposedQueue<TObservation> _memorizedObservations;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryBelief{TReference, TObservation}"/> class with an object
        /// reference, a function to generate/update the observation using the object reference, and a condition on when
        /// the observation should be updated. Also initializes the memory array with a specified number of slots.
        /// </summary>
        /// <param name="metadata">
        /// Metadata about this Belief, used to quickly display the goal in several contexts.
        /// </param>
        /// <param name="reference">
        /// The reference used to generate/update the observation. This <i>must</i> be a reference type, be aware that
        /// this is not enforced by C# if <typeparamref name="TReference"/> is an interface.
        /// </param>
        /// <param name="getObservationFromReference">
        /// A function that takes a reference and generates/updates a observation.
        /// </param>
        /// <param name="framesToRemember">The number of frames to remember back.</param>
        /// <param name="shouldUpdate">
        /// A function that sets a condition on when the observation should be updated.
        /// </param>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="reference"/> is not a reference type.
        /// </exception>
        public MemoryBelief(
            Metadata metadata,
            TReference reference,
            Func<TReference, TObservation> getObservationFromReference,
            int framesToRemember,
            Func<bool> shouldUpdate
        )
            : base(metadata, reference, getObservationFromReference, shouldUpdate)
            => _memorizedObservations = new ExposedQueue<TObservation>(framesToRemember);

        /// <inheritdoc cref="MemoryBelief{TReference,TObservation}(Metadata,TReference,Func{TReference,TObservation},int,Func{bool})"/>
        public MemoryBelief(
            TReference reference,
            Func<TReference, TObservation> getObservationFromReference,
            int framesToRemember,
            Func<bool> shouldUpdate
        )
            : this(new Metadata(), reference, getObservationFromReference, framesToRemember, shouldUpdate)
        {
        }

        /// <inheritdoc
        ///     cref="MemoryBelief{TReference,TObservation}(Metadata,TReference,Func{TReference,TObservation},int,Func{bool})" />
        public MemoryBelief(
            Metadata metadata,
            TReference reference,
            Func<TReference, TObservation> getObservationFromReference,
            int framesToRemember
        )
            : this(metadata, reference, getObservationFromReference, framesToRemember, () => true)
        {
        }

        /// <inheritdoc
        ///     cref="MemoryBelief{TReference,TObservation}(Metadata,TReference,Func{TReference,TObservation},int,Func{bool})" />
        public MemoryBelief(TReference reference,
            Func<TReference, TObservation> getObservationFromReference,
            int framesToRemember)
            : this(new Metadata(), reference, getObservationFromReference, framesToRemember, () => true)
        {
        }

        /// <summary>
        /// Generates/updates the observation.
        /// Also stores the previous observation in memory.
        /// </summary>
        public override void UpdateBelief()
        {
            // We use the implicit conversion to TObservation to store the observation.
            _memorizedObservations.Put(this);

            if (_shouldUpdate()) UpdateObservation();
        }

        /// <summary>
        /// Gets the most recently memorized observation.
        /// </summary>
        /// <returns> The most recent memory of the observation.</returns>
        public TObservation GetMostRecentMemory() => _memorizedObservations.GetFirst();

        /// <summary>
        /// Gets the memorized observation at a specific index.
        /// A higher index means a memory further back in time.
        /// </summary>
        /// <returns>The memory of the observation at the specified index.</returns>
        /// <param name="index">The index of the memory to get.</param>
        /// <param name="clamp">If true, the index will be clamped between 0 and the last memory index.</param>
        public TObservation GetMemoryAt(int index, bool clamp = false)
        {
            int lastMemoryIndex = _memorizedObservations.Count - 1;
            if (clamp)
                index = Math.Clamp(index, 0, lastMemoryIndex);
            else if (index < 0 || index > lastMemoryIndex)
                throw new ArgumentOutOfRangeException(nameof(index), $"Index must be between 0 and {lastMemoryIndex}.");
            return _memorizedObservations[index];
        }

        /// <summary>
        /// Gets all the memorized observations.
        /// The first element is the newest memory.
        /// </summary>
        /// <returns> An array of all the memorized observations.</returns>
        public TObservation[] GetAllMemories() => _memorizedObservations.ToArray();
    }
}
