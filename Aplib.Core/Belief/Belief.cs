using System;

namespace Aplib.Core.Belief
{
    /// <summary>
    /// The <see cref="Belief{TReference, TObservation}"/> class represents the agent's belief of a single object.
    /// Some <i>object reference</i> is used to generate/update an <i>observation</i> 
    /// (i.e., some piece of information on the game state as perceived by an agent).
    /// </summary>
    /// <remarks>
    /// It implements the <see cref="IBelief"/> interface.
    /// It supports implicit conversion to <typeparamref name="TObservation"/>.
    /// </remarks>
    /// <typeparam name="TReference">The type of the object reference used to generate/update the observation.</typeparam>
    /// <typeparam name="TObservation">The type of the observation that the belief represents.</typeparam>
    public class Belief<TReference, TObservation> : IBelief
    {
        /// <summary>
        /// The object reference used to generate/update the observation.
        /// </summary>
        private readonly TReference _reference;

        /// <summary>
        /// A function that takes an object reference and generates/updates an observation.
        /// </summary>
        private readonly Func<TReference, TObservation> _getObservationFromReference;

        /// <summary>
        /// A condition on when the observation should be updated.
        /// </summary>
        private readonly Func<bool> _shouldUpdate = () => true;

        /// <summary>
        /// The observation represented by the belief (i.e., some piece of information on the game state as perceived by an agent).
        /// </summary>
        private TObservation _observation;

        /// <summary>
        /// Initializes a new instance of the <see cref="Belief{TReference, TObservation}"/> class with an object reference,
        /// and a function to generate/update the observation using the object reference.
        /// </summary>
        /// <param name="reference">A function that takes an object reference and generates/updates an observation.</param>
        /// <param name="getObservationFromReference">A function that takes an object reference and generates/updates an observation.</param>
        public Belief(TReference reference, Func<TReference, TObservation> getObservationFromReference)
        {
            _reference = reference;
            _getObservationFromReference = getObservationFromReference;
            _observation = _getObservationFromReference(_reference);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Belief{TReference, TObservation}"/> class with an object reference,
        /// a function to generate/update the observation using the object reference,
        /// and a condition on when the observation should be updated.
        /// </summary>
        /// <param name="reference">The object reference used to generate/update the observation.</param>
        /// <param name="getObservationFromReference">A function that takes an object reference and generates/updates an observation.</param>
        /// <param name="shouldUpdate">A condition on when the observation should be updated.</param>
        public Belief(TReference reference, Func<TReference, TObservation> getObservationFromReference, Func<bool> shouldUpdate)
            : this(reference, getObservationFromReference)
        {
            _shouldUpdate = shouldUpdate;
        }

        /// <summary>
        /// Implicit conversion operator to allow a <see cref="Belief{TReference, TObservation}"/> object 
        /// to be used where a <typeparamref name="TObservation"/> is expected.
        /// </summary>
        /// <param name="belief">The <see cref="Belief{TReference, TObservation}"/> object to convert.</param>
        public static implicit operator TObservation(Belief<TReference, TObservation> belief) => belief._observation;

        /// <summary>
        /// Generates/updates the observation if the updateIf condition is satisfied.
        /// The observation is then updated by calling the getObservationFromReference function.
        /// </summary>
        public void UpdateBelief()
        {
            if (_shouldUpdate()) _observation = _getObservationFromReference(_reference);
        }
    }
}
