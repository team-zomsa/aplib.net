using System;

namespace Aplib.Core.Belief.Beliefs
{
    /// <summary>
    /// The <see cref="Belief{TReference, TObservation}"/> class represents the agent's belief of a single object.
    /// Some <i>object reference</i> is used to generate/update an <i>observation</i>
    /// (i.e., some piece of information of the game state as perceived by an agent).
    /// </summary>
    /// <remarks>
    /// It supports implicit conversion to <typeparamref name="TObservation"/>.
    /// </remarks>
    /// <typeparam name="TReference">
    /// The type of the object reference used to generate/update the observation. This <i>must</i> be a reference type,
    /// be aware that this is not enforced by C# if <typeparamref name="TReference"/> is an interface.
    /// </typeparam>
    /// <typeparam name="TObservation">The type of the observation that the belief represents.</typeparam>
    public class Belief<TReference, TObservation> : IBelief where TReference : class
    {
        /// <summary>
        /// The object reference used to generate/update the observation.
        /// </summary>
        protected readonly TReference _reference;

        /// <summary>
        /// A function that takes an object reference and generates/updates an observation.
        /// </summary>
        protected readonly Func<TReference, TObservation> _getObservationFromReference;

        /// <summary>
        /// A condition on when the observation should be updated.
        /// </summary>
        protected readonly Func<bool> _shouldUpdate;

        /// <summary>
        /// Gets the metadata of the Belief.
        /// </summary>
        /// <remark>
        /// This metadata may be useful for debugging or logging.
        /// </remark>
        public Metadata Metadata { get; }

        /// <summary>
        /// The observation represented by the belief (i.e., some piece of information of the game state as perceived by an agent).
        /// </summary>
        public TObservation Observation { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Belief{TReference, TObservation}"/> class with an object
        /// reference, a function to generate/update the observation using the object reference,
        /// and a condition on when the observation should be updated.
        /// </summary>
        /// <param name="metadata">
        /// Metadata about this Belief, used to quickly display the goal in several contexts.
        /// </param>
        /// <param name="reference">
        /// The object reference used to generate/update the observation. This <i>must</i> be a reference type, be aware
        /// that this is not enforced by C# if <typeparamref name="TReference"/> is an interface.
        /// </param>
        /// <param name="getObservationFromReference">
        /// A function that takes an object reference and generates/updates an observation.
        /// </param>
        /// <param name="shouldUpdate">A condition on when the observation should be updated.</param>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="reference"/> is not a reference type.
        /// </exception>
        public Belief
        (
            Metadata metadata,
            TReference reference,
            Func<TReference, TObservation> getObservationFromReference,
            Func<bool> shouldUpdate
        )
        {
            Type referenceType = reference.GetType();
            if (referenceType.IsValueType)
                throw new ArgumentException($"{referenceType.FullName} is not a reference type.", nameof(reference));

            Metadata = metadata;
            _reference = reference;
            _getObservationFromReference = getObservationFromReference;
            Observation = _getObservationFromReference(_reference);
            _shouldUpdate = shouldUpdate;
        }

        /// <inheritdoc
        ///     cref="Belief{TReference,TObservation}(Aplib.Core.Metadata,TReference,Func{TReference,TObservation},Func{bool})"/>
        public Belief
        (
            TReference reference,
            Func<TReference, TObservation> getObservationFromReference,
            Func<bool> shouldUpdate
        )
            : this(new Metadata(), reference, getObservationFromReference, shouldUpdate)
        {
        }

        /// <inheritdoc
        ///     cref="Belief{TReference,TObservation}(Aplib.Core.Metadata,TReference,Func{TReference,TObservation},Func{bool})" />
        public Belief
        (
            Metadata metadata,
            TReference reference,
            Func<TReference, TObservation> getObservationFromReference
        )
            : this(metadata, reference, getObservationFromReference, () => true)
        {
        }

        /// <inheritdoc
        ///     cref="Belief{TReference,TObservation}(Aplib.Core.Metadata,TReference,Func{TReference,TObservation},Func{bool})" />
        public Belief(TReference reference, Func<TReference, TObservation> getObservationFromReference)
            : this(new Metadata(), reference, getObservationFromReference, () => true)
        {
        }

        /// <summary>
        /// Implicit conversion operator to allow a <see cref="Belief{TReference, TObservation}"/> object
        /// to be used where a <typeparamref name="TObservation"/> is expected.
        /// </summary>
        /// <param name="belief">The <see cref="Belief{TReference, TObservation}"/> object to convert.</param>
        public static implicit operator TObservation(Belief<TReference, TObservation> belief) => belief.Observation;

        /// <summary>
        /// Generates/updates the observation if the shouldUpdate condition is satisfied.
        /// The observation is then updated by calling the getObservationFromReference function.
        /// </summary>
        public virtual void UpdateBelief()
        {
            if (_shouldUpdate()) UpdateObservation();
        }

        /// <summary>
        /// Generates/updates the observation.
        /// </summary>
        protected void UpdateObservation()
            => Observation = _getObservationFromReference(_reference);
    }
}
