using Aplib.Core;
using Aplib.Core.Belief.BeliefSets;
using System;

namespace Aplib.Extensions.Actions
{
    public class PathfinderAction<TBeliefset, TLocation> : Core.Intent.Actions.Action<TBeliefset>
        where TBeliefset : IBeliefSet
    {
        public PathfinderAction(IMetadata metadata,
            IPathfinder<TLocation> pathfinder,
            Func<TBeliefset, TLocation> getCurrentLocation,
            Func<TBeliefset, TLocation> getEndLocation,
            Action<TBeliefset, TLocation> effect)
            : base(metadata, ExecutePathFinder(pathfinder, getCurrentLocation, getEndLocation, effect))
        {
        }

        public PathfinderAction(IPathfinder<TLocation> pathfinder,
            Func<TBeliefset, TLocation> getCurrentLocation,
            Func<TBeliefset, TLocation> getEndLocation,
            Action<TBeliefset, TLocation> effect)
            : this(new Metadata(), pathfinder, getCurrentLocation, getEndLocation, effect)
        {
        }

        private static Action<TBeliefset> ExecutePathFinder(IPathfinder<TLocation> pathfinder,
            Func<TBeliefset, TLocation> getCurrentLocation,
            Func<TBeliefset, TLocation> getEndLocation,
            Action<TBeliefset, TLocation> effect) => beliefSet =>
        {
            TLocation startLocation = getCurrentLocation(beliefSet);
            TLocation endLocation = getEndLocation(beliefSet);

            if (!pathfinder.TryGetNextStep(startLocation, endLocation, out TLocation nextLocation))
            {
                return;
            }

            effect(beliefSet, nextLocation);
        };
    }
}
