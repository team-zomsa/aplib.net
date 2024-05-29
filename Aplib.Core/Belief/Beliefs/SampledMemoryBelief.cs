using System;
using static Aplib.Core.Belief.Beliefs.UpdateMode;

namespace Aplib.Core.Belief.Beliefs
{
    /// <summary>
    /// The <see cref="SampledMemoryBelief{TReference, TObservation}"/> class represents the agent's belief of a single object,
    /// but with additional "memory" of previous observations.
    /// These observations are sampled at a fixed rate.
    /// Some <i>object reference</i> is used to generate/update an <i>observation</i>
    /// (i.e., some piece of information on the game state as perceived by an agent).
    /// This belief also stores a limited amount of previous observation samples in memory.
    /// Optionally, the belief can always store the most recent observation, regardless of the sample rate.
    /// </summary>
    /// <remarks>
    /// It supports implicit conversion to <typeparamref name="TObservation"/>.
    /// </remarks>
    /// <typeparam name="TReference">
    /// The type of the reference used to generate/update the observation. This <i>must</i> be a reference type, be
    /// aware that this is not enforced by C# if <typeparamref name="TReference"/> is an interface.
    /// </typeparam>
    /// <typeparam name="TObservation">The type of the observation the belief represents.</typeparam>
    public class SampledMemoryBelief<TReference, TObservation> : MemoryBelief<TReference, TObservation>
        where TReference : class
    {
        /// <summary>
        /// The sample interval of the memory (inverse of the sample rate).
        /// One observation memory (i.e., snapshot) is stored every <see cref="_sampleInterval"/>-th cycle.
        /// </summary>
        private readonly int _sampleInterval;

        /// <summary>
        /// Specifies how this sampled memory belief should be updated.
        /// </summary>
        private readonly UpdateMode _updateMode;

        private int _moduloCounter = 0;

        /// <summary>
        /// The number of cycles that have passed since the last memory sample was stored.
        /// </summary>
        private int ModuloCounter
        {
            get => _moduloCounter;
            set => _moduloCounter = value % _sampleInterval;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SampledMemoryBelief{TReference, TObservation}"/> class with an
        /// object reference, a function to generate/update the observation using the object reference, and a condition
        /// on when the observation should be updated. This belief also stores a limited amount of previous observation
        /// samples in memory.
        /// Optionally, the belief can always store the most recent observation, regardless of the sample rate.
        /// </summary>
        /// <param name="metadata">
        /// Metadata about this goal, used to quickly display the goal in several contexts.
        /// </param>
        /// <param name="reference">
        /// The reference used to generate/update the observation. This <i>must</i> be a reference type.
        /// </param>
        /// <param name="getObservationFromReference">
        /// A function that takes a reference and generates/updates an observation.
        /// </param>
        /// <param name="sampleInterval">
        /// The sample interval of the memory.
        /// One observation memory (i.e., snapshot) is stored every <c>sampleInterval</c>-th cycle.
        /// </param>
        /// <param name="updateMode">Specifies how this sampled memory belief should be updated.</param>
        /// <param name="framesToRemember">The number of frames to remember back.</param>
        /// <param name="shouldUpdate">
        /// A function that sets a condition on when the observation should be updated.
        /// </param>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="reference"/> is not a reference type.
        /// </exception>
        public SampledMemoryBelief
        (
            Metadata metadata,
            TReference reference,
            Func<TReference, TObservation> getObservationFromReference,
            int sampleInterval,
            UpdateMode updateMode,
            int framesToRemember,
            Func<bool> shouldUpdate
        )
            : base(metadata, reference, getObservationFromReference, framesToRemember, shouldUpdate)
        {
            _sampleInterval = sampleInterval;
            _updateMode = updateMode;
        }

        /// <inheritdoc
        ///     cref="SampledMemoryBelief{TReference,TObservation}(Metadata,TReference,Func{TReference,TObservation},int,UpdateMode,int,Func{bool})" />
        public SampledMemoryBelief
        (
            TReference reference,
            Func<TReference, TObservation> getObservationFromReference,
            int sampleInterval,
            UpdateMode updateMode,
            int framesToRemember,
            Func<bool> shouldUpdate
        )
            : this
            (
                new Metadata(),
                reference,
                getObservationFromReference,
                sampleInterval,
                updateMode,
                framesToRemember,
                shouldUpdate
            )
        {
        }

        /// <inheritdoc
        ///     cref="SampledMemoryBelief{TReference,TObservation}(Metadata,TReference,Func{TReference,TObservation},int,UpdateMode,int,Func{bool})" />
        public SampledMemoryBelief
        (
            Metadata metadata,
            TReference reference,
            Func<TReference, TObservation> getObservationFromReference,
            int sampleInterval,
            UpdateMode updateMode,
            int framesToRemember
        )
            : this
            (
                metadata,
                reference,
                getObservationFromReference,
                sampleInterval,
                updateMode,
                framesToRemember,
                () => true
            )
        {
        }

        /// <inheritdoc
        ///     cref="SampledMemoryBelief{TReference,TObservation}(Metadata,TReference,Func{TReference,TObservation},int,UpdateMode,int,Func{bool})" />
        public SampledMemoryBelief
        (
            TReference reference,
            Func<TReference, TObservation> getObservationFromReference,
            int sampleInterval,
            UpdateMode updateMode,
            int framesToRemember
        )
            : this
            (
                new Metadata(),
                reference,
                getObservationFromReference,
                sampleInterval,
                updateMode,
                framesToRemember,
                () => true
                )
        {
        }

        /// <summary>
        /// Determines whether the memory should be sampled.
        /// One observation memory (i.e., snapshot) is stored every <c>sampleInterval</c>-th cycle.
        /// </summary>
        /// <returns>Whether a memory sample should be stored in the current cycle.</returns>
        private bool ShouldSampleMemory() => ModuloCounter++ == 0;

        /// <summary>
        /// Generates/updates the observation if applicable.
        /// Also stores the previous observation in memory every <c>sampleInterval</c>-th cycle.
        /// </summary>
        public override void UpdateBelief()
        {
            if (ShouldSampleMemory())
                base.UpdateBelief();
            else if (_updateMode is AlwaysUpdate && _shouldUpdate()) UpdateObservation();
        }
    }
}
