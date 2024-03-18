using System;

namespace Aplib.Core.Belief
{
    public class Belief<TReference, TResource> : IBelief
    {
        private readonly TReference _reference;

        private readonly Func<TReference, TResource> _getResourceFromReference;

        private readonly Func<bool> _updateIf = () => true;

        private TResource _resource;

        public Belief(TReference reference, Func<TReference, TResource> getResourceFromReference)
        {
            _reference = reference;
            _getResourceFromReference = getResourceFromReference;
            _resource = _getResourceFromReference(_reference);
        }

        public Belief(TReference reference, Func<TReference, TResource> getResourceFromReference, Func<bool> updateIf)
            : this(reference, getResourceFromReference)
        {
            _updateIf = updateIf;
        }

        public static implicit operator TResource(Belief<TReference, TResource> belief) => belief._resource;

        public void UpdateBelief()
        {
            if (_updateIf()) _resource = _getResourceFromReference(_reference);
        }
    }
}
