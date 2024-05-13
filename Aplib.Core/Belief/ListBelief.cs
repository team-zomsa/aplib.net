using System;
using System.Collections.Generic;
using System.Linq;

namespace Aplib.Core.Belief
{
    /// <summary>
    /// A convenience variant of <see cref="Belief{TReference,TObservation}" /> to track multiple references in one
    /// belief. Both the collection storing the references and the references themselves can be changed after the
    /// <see cref="ListBelief{TReference,TObservation}" /> has been created.
    /// </summary>
    /// <remarks>
    /// A <see cref="ListBelief{TReference,TObservation}" /> can be implicitly converted to a
    /// <see cref="List{TObservation}" />, which will have the same size as the reference collection the last time that
    /// <see cref="Belief{TReference,TObservation}.UpdateBelief" /> was called, and contain the observation results for
    /// each element in the collection.
    /// </remarks>
    /// <typeparam name="TReference">
    /// The type of the object references used to generate/update the observation.
    /// </typeparam>
    /// <typeparam name="TObservation">The type of the observations that the belief represents.</typeparam>
    public class ListBelief<TReference, TObservation> : Belief<IEnumerable<TReference>, List<TObservation>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListBelief{TReference,TObservation}" /> class from an object
        /// reference collection, and a function to generate an observation from an object reference.
        /// </summary>
        /// <param name="references">
        /// The collection of reference objects. The underlying type implementing <see cref="IEnumerable{TReference}" />
        /// <i>must</i> be a reference type, note that this is not enforced by C#.
        /// </param>
        /// <param name="getObservationFromReference">
        /// A function that takes an object reference and generates an observation.
        /// </param>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="references"/> is not a reference type.
        /// </exception>
        public ListBelief
        (
            IEnumerable<TReference> references,
            Func<TReference, TObservation> getObservationFromReference
        )
            : base(references, refer => refer.Select(getObservationFromReference).ToList())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListBelief{TReference,TObservation}" /> class from an object
        /// reference collection, a function to generate an observation from an object reference, and an update guard.
        /// </summary>
        /// <param name="references">
        /// The collection of reference objects. The underlying type implementing <see cref="IEnumerable{TReference}" />
        /// <i>must</i> be a reference type, note that this is not enforced by C#.
        /// </param>
        /// <param name="getObservationFromReference">
        /// A function that takes an object reference and generates an observation.
        /// </param>
        /// <param name="shouldUpdate">A condition on when the observation should be updated.</param>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="references"/> is not a reference type.
        /// </exception>
        public ListBelief
        (
            IEnumerable<TReference> references,
            Func<TReference, TObservation> getObservationFromReference,
            Func<bool> shouldUpdate
        )
            : base(references, refer => refer.Select(getObservationFromReference).ToList(), shouldUpdate)
        {
        }
    }
}