using System;

namespace Aplib.Core.Stubs
{
    public class Belief<TReference, TResource> : IBelief
    {
        private readonly TReference _reference;
        private TResource _resource;

        private readonly Func<bool> _updateIf = () => true;
        private readonly Func<TReference, TResource> _getResourceFromReference;

        public Belief(TReference reference, Func<TReference, TResource> getResourceFromReference)
        {
            _reference = reference;
            _getResourceFromReference = getResourceFromReference;
            _resource = _getResourceFromReference(_reference);
        }

        public Belief(TReference reference, Func<TReference, TResource> getResourceFromReference, Func<bool> updateIf)
            : this(reference, getResourceFromReference) => _updateIf = updateIf;

        public void UpdateBelief()
        {
            if (_updateIf())
                _resource = _getResourceFromReference(_reference);
        }

        public static implicit operator TResource(Belief<TReference, TResource> belief) => belief._resource;
    }
}
