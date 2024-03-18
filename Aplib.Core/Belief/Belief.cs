using System;

namespace Aplib.Core.Belief
{
    /// <summary>
    /// The <see cref="Belief{TReference, TResource}"/> class represents a belief.
    /// Some <i>object reference</i> is used to generate/update a <i>resource</i> 
    /// (i.e., some piece of information on the game state as perceived by an agent).
    /// </summary>
    /// <remarks>
    /// It implements the <see cref="IBelief"/> interface.
    /// It supports implicit conversion to <typeparamref name="TResource"/>.
    /// </remarks>
    /// <typeparam name="TReference">The type of the reference used to generate/update the resource.</typeparam>
    /// <typeparam name="TResource">The type of the resource the belief represents.</typeparam>
    public class Belief<TReference, TResource> : IBelief
    {
        /// <summary>
        /// The reference used to generate/update the resource.
        /// </summary>
        private readonly TReference _reference;

        /// <summary>
        /// A function that takes a reference and generates/updates a resource.
        /// </summary>
        private readonly Func<TReference, TResource> _getResourceFromReference;

        /// <summary>
        /// A function that sets a condition on when the resource should be updated.
        /// </summary>
        private readonly Func<bool> _updateIf = () => true;

        /// <summary>
        /// The resource represented by the belief (i.e., some piece of information on the game state as perceived by an agent).
        /// </summary>
        private TResource _resource;

        /// <summary>
        /// Initializes a new instance of the <see cref="Belief{TReference, TResource}"/> class with a reference, 
        /// and a function to generate/update the resource using the reference.
        /// </summary>
        /// <param name="reference">The reference used to generate/update the resource.</param>
        /// <param name="getResourceFromReference">A function that takes a reference and generates/updates a resource.</param>
        public Belief(TReference reference, Func<TReference, TResource> getResourceFromReference)
        {
            _reference = reference;
            _getResourceFromReference = getResourceFromReference;
            _resource = _getResourceFromReference(_reference);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Belief{TReference, TResource}"/>
        /// </summary>
        /// <param name="reference">The reference used to generate/update the resource.</param>
        /// <param name="getResourceFromReference">A function that takes a reference and generates/updates a resource.</param>
        /// <param name="updateIf">A function that sets a condition on when the resource should be updated.</param>
        public Belief(TReference reference, Func<TReference, TResource> getResourceFromReference, Func<bool> updateIf)
            : this(reference, getResourceFromReference)
        {
            _updateIf = updateIf;
        }

        /// <summary>
        /// Implicit conversion operator to allow a <see cref="Belief{TReference, TResource}"/> object 
        /// to be used where a <typeparamref name="TResource"/> is expected.
        /// </summary>
        /// <param name="belief">The <see cref="Belief{TReference, TResource}"/> object to convert.</param>
        public static implicit operator TResource(Belief<TReference, TResource> belief) => belief._resource;

        /// <summary>
        /// Generates/updates the resource if the updateIf condition is satisfied.
        /// The resource is then updated by calling the getResourceFromReference function.
        /// </summary>
        public void UpdateBelief()
        {
            if (_updateIf()) _resource = _getResourceFromReference(_reference);
        }
    }
}
