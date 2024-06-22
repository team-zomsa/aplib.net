using Aplib.Core;
using Aplib.Core.Belief.BeliefSets;
using Aplib.Core.Intent.Actions;

namespace Aplib.Extensions.Actions
{
    /// <summary>
    /// Represents an action that uses a pathfinding algorithm to determine the next step in a belief set.
    /// </summary>
    /// <typeparam name="TBeliefSet">The type of the belief set.</typeparam>
    /// <typeparam name="TLocation">The type of the location.</typeparam>
    public class PathfinderAction<TBeliefSet, TLocation> : Action<TBeliefSet>
        where TBeliefSet : IBeliefSet
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PathfinderAction{TBeliefSet, TLocation}"/> class.
        /// </summary>
        /// <param name="metadata">The metadata of the action.</param>
        /// <param name="pathfinder">The pathfinder that finds the path through the world.</param>
        /// <param name="getCurrentLocation">The function that gets the current location in the world.</param>
        /// <param name="getTargetLocation">The function that gets the destination in the world.</param>
        /// <param name="effectWithNextStep">The function that does something with the found location.</param>
        public PathfinderAction(IMetadata metadata,
            IPathfinder<TLocation> pathfinder,
            System.Func<TBeliefSet, TLocation> getCurrentLocation,
            System.Func<TBeliefSet, TLocation> getTargetLocation,
            System.Action<TBeliefSet, TLocation> effectWithNextStep)
            : base(metadata, ExecutePathFinder(pathfinder, getCurrentLocation, getTargetLocation, effectWithNextStep))
        {
        }

        /// <inheritdoc />
        public PathfinderAction(IPathfinder<TLocation> pathfinder,
            System.Func<TBeliefSet, TLocation> getCurrentLocation,
            System.Func<TBeliefSet, TLocation> getTargetLocation,
            System.Action<TBeliefSet, TLocation> effectWithNextStep)
            : this(new Metadata(), pathfinder, getCurrentLocation, getTargetLocation, effectWithNextStep)
        {
        }

        private static System.Action<TBeliefSet> ExecutePathFinder(IPathfinder<TLocation> pathfinder,
            System.Func<TBeliefSet, TLocation> getCurrentLocation,
            System.Func<TBeliefSet, TLocation> getTargetLocation,
            System.Action<TBeliefSet, TLocation> effectWithNextStep) => beliefSet =>
        {
            TLocation currentLocation = getCurrentLocation(beliefSet);
            TLocation targetLocation = getTargetLocation(beliefSet);

            if (pathfinder.TryGetNextStep(currentLocation, targetLocation, out TLocation nextLocation))
                effectWithNextStep(beliefSet, nextLocation);
        };
    }
}
