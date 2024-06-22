using Aplib.Core;
using Aplib.Core.Belief.BeliefSets;
using Aplib.Core.Intent.Actions;

namespace Aplib.Extensions.Actions
{
    public class PathfinderAction<TBeliefset, T> : Action<TBeliefset>
        where TBeliefset : IBeliefSet
    {
        public PathfinderAction(IMetadata metadata, IPathfinder<T> pathfinder, System.Func<TBeliefset, T> getCurrentLocation, System.Func<TBeliefset, T> getEndLocation, System.Action<TBeliefset, T> effect)
            : base(metadata, ExecutePathFinder(pathfinder, getCurrentLocation, getEndLocation, effect))
        {
        }

        public PathfinderAction(IPathfinder<T> pathfinder, System.Func<TBeliefset, T> getCurrentLocation, System.Func<TBeliefset, T> getEndLocation, System.Action<TBeliefset, T> effect)
            : this(null, pathfinder, getCurrentLocation, getEndLocation, effect)
        {
        }

        private static System.Action<TBeliefset> ExecutePathFinder(IPathfinder<T> pathfinder,System.Func<TBeliefset, T> getCurrentLocation, System.Func<TBeliefset, T> getEndLocation, System.Action<TBeliefset, T> effect) => (set =>
        {
            T startLocation = getCurrentLocation(set);
            T endLocation = getEndLocation(set);

            if (!pathfinder.TryGetNextStep(startLocation, endLocation, out T nextLocation))
            {
                return;
            }

            effect(set, nextLocation);
        });
    }
}
