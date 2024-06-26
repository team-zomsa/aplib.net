// This program has been developed by students from the bachelor Computer Science at Utrecht
// University within the Software Project course.
// Copyright Utrecht University (Department of Information and Computing Sciences)

namespace Aplib.Core.Belief.Beliefs
{
    /// <summary>
    /// Represents the agent's belief of a single object.
    /// Some <i>object reference</i> is used to generate/update an <i>observation</i>
    /// (i.e., some information about the game state as perceived by an agent).
    /// </summary>
    /// <remarks>
    /// This class supports implicit conversion to <typeparamref name="TObservation" />.
    /// </remarks>
    /// <typeparam name="TReference">
    /// The type of the object reference used to generate/update the observation.
    /// This <i>must</i> be a reference type,
    /// be aware that this is not enforced by the type checker if <typeparamref name="TReference" /> is an interface.
    /// </typeparam>
    /// <typeparam name="TObservation">The type of the observation that the belief represents.</typeparam>
    public class Belief<TReference, TObservation> : IBelief where TReference : class
    {
        /// <summary>
        /// Gets the metadata of the Belief.
        /// </summary>
        /// <remark>
        /// This metadata may be useful for debugging or logging.
        /// </remark>
        public Metadata Metadata { get; }

        /// <summary>
        /// The observation represented by the belief
        /// (i.e., some information about the game state as perceived by an agent).
        /// </summary>
        public TObservation Observation { get; protected set; }

        /// <summary>
        /// A function that takes an object reference and generates/updates an observation.
        /// </summary>
        protected internal readonly System.Func<TReference, TObservation> _getObservationFromReference;

        /// <summary>
        /// The object reference used to generate/update the observation.
        /// </summary>
        protected internal readonly TReference _reference;

        /// <summary>
        /// A condition on when the observation should be updated. Takes the object reference
        /// of the belief as a parameter for the predicate.
        /// </summary>
        protected internal readonly System.Predicate<TReference> _shouldUpdate;

        /// <summary>
        /// Initializes a new instance of the <see cref="Belief{TReference, TObservation}" /> class with an object
        /// reference, a function to generate/update the observation using the object reference,
        /// and optionally a condition on when the observation should be updated.
        /// </summary>
        /// <param name="metadata">
        /// Optional metadata about this belief, used to quickly display the goal in several contexts.
        /// If omitted, default metadata will be used.
        /// </param>
        /// <param name="reference">
        /// The object reference used to generate/update the observation. This <i>must</i> be a reference type, be aware
        /// that this is not enforced by the type checker if <typeparamref name="TReference"/> is an interface.
        /// </param>
        /// <param name="getObservationFromReference">
        /// A function that takes an object reference and generates/updates an observation.
        /// </param>
        /// <param name="shouldUpdate">
        /// An optional condition on when the observation should be updated.
        /// Takes the object reference of the belief as a parameter for the predicate.
        /// If omitted, the belief will always update.
        /// </param>
        /// <exception cref="System.ArgumentException">
        /// Thrown when <paramref name="reference" /> is not a reference type.
        /// </exception>
        public Belief
        (
            Metadata metadata,
            TReference reference,
            System.Func<TReference, TObservation> getObservationFromReference,
            System.Predicate<TReference> shouldUpdate
        )
        {
            System.Type referenceType = reference.GetType();
            if (referenceType.IsValueType)
                throw new System.ArgumentException
                    ($"{referenceType.FullName} is not a reference type.", nameof(reference));

            Metadata = metadata;
            _reference = reference;
            _getObservationFromReference = getObservationFromReference;
            Observation = _getObservationFromReference(_reference);
            _shouldUpdate = shouldUpdate;
        }

        /// <inheritdoc
        ///     cref="Belief{TReference,TObservation}(Core.Metadata,TReference,System.Func{TReference,TObservation},System.Predicate{TReference})" />
        public Belief
        (
            TReference reference,
            System.Func<TReference, TObservation> getObservationFromReference,
            System.Predicate<TReference> shouldUpdate
        )
            : this(new Metadata(), reference, getObservationFromReference, shouldUpdate)
        {
        }

        /// <inheritdoc
        ///     cref="Belief{TReference,TObservation}(Core.Metadata,TReference,System.Func{TReference,TObservation},System.Predicate{TReference})" />
        public Belief
        (
            Metadata metadata,
            TReference reference,
            System.Func<TReference, TObservation> getObservationFromReference
        )
            : this(metadata, reference, getObservationFromReference, _ => true)
        {
        }

        /// <inheritdoc
        ///     cref="Belief{TReference,TObservation}(Core.Metadata,TReference,System.Func{TReference,TObservation},System.Predicate{TReference})" />
        public Belief(TReference reference, System.Func<TReference, TObservation> getObservationFromReference)
            : this(new Metadata(), reference, getObservationFromReference, _ => true)
        {
        }

        /// <summary>
        /// Implicit conversion operator to allow a <see cref="Belief{TReference, TObservation}" /> object
        /// to be used where a <typeparamref name="TObservation" /> is expected.
        /// </summary>
        /// <param name="belief">The <see cref="Belief{TReference, TObservation}" /> object to convert.</param>
        public static implicit operator TObservation(Belief<TReference, TObservation> belief) => belief.Observation;

        /// <summary>
        /// Generates/updates the observation if the <c>shouldUpdate</c> condition is satisfied.
        /// The observation is then updated by calling the <c>getObservationFromReference</c> function.
        /// </summary>
        public virtual void UpdateBelief()
        {
            if (_shouldUpdate(_reference)) UpdateObservation();
        }

        /// <summary>
        /// Generates/updates the observation.
        /// </summary>
        protected void UpdateObservation()
            => Observation = _getObservationFromReference(_reference);
    }
}
