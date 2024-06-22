using Aplib.Core;
using Aplib.Core.Belief.BeliefSets;
using System;

namespace Aplib.Extensions.Actions
{
    public class PathfinderAction<TBeliefset, T> : Core.Intent.Actions.Action<TBeliefset>
        where TBeliefset : IBeliefSet
    {
        public PathfinderAction(IMetadata metadata,
            IPathfinder<T> pathfinder,
            Func<TBeliefset, T> getCurrentLocation,
            Func<TBeliefset, T> getEndLocation,
            Action<TBeliefset, T> effect)
            : base(metadata, ExecutePathFinder(pathfinder, getCurrentLocation, getEndLocation, effect))
        {
        }

        public PathfinderAction(IPathfinder<T> pathfinder,
            Func<TBeliefset, T> getCurrentLocation,
            Func<TBeliefset, T> getEndLocation,
            Action<TBeliefset, T> effect)
            : this(null, pathfinder, getCurrentLocation, getEndLocation, effect)
        {
        }

        private static Action<TBeliefset> ExecutePathFinder(IPathfinder<T> pathfinder,
            Func<TBeliefset, T> getCurrentLocation,
            Func<TBeliefset, T> getEndLocation,
            Action<TBeliefset, T> effect) => set =>
        {
            T startLocation = getCurrentLocation(set);
            T endLocation = getEndLocation(set);

            if (!pathfinder.TryGetNextStep(startLocation, endLocation, out T nextLocation))
            {
                return;
            }

            effect(set, nextLocation);
        };
    }
}
