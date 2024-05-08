using System;

namespace Aplib.Extensions
{
    public interface IPathfinder<T>
    {
        public ReadOnlySpan<T> FindPath(T begin, T end);

        public T GetNextStep(T current, T end);

        public bool TryGetNextStep(T current, T end, out T nextStep);
    }
}
