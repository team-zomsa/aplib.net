using Aplib.Core.Belief.BeliefSets;
using System;

namespace Aplib.Core.Desire.Goals
{
    /// <summary>
    /// Contains helper methods to generate commonly used heuristic functions.
    /// </summary>
    public static class CommonHeuristicFunctions<TBeliefSet>
        where TBeliefSet : IBeliefSet
    {
        /// <summary>
        /// Converts a boolean-based heuristic function to a <see cref="Goal{TBeliefSet}.HeuristicFunction"/>.
        /// </summary>
        /// <param name="heuristicFunction">
        /// A heuristic function which returns true only when the state is considered completed.
        /// </param>
        /// <returns>A heuristic function which wraps around the boolean-based heuristic function.</returns>
        public static Goal<TBeliefSet>.HeuristicFunction Boolean(Func<TBeliefSet, bool> heuristicFunction)
            => beliefSet => Heuristics.Boolean(heuristicFunction.Invoke(beliefSet));

        /// <summary>
        /// A <see cref="Goal{TBeliefSet}.HeuristicFunction"/> which always returns <see cref="Heuristics"/> with the same distance.
        /// </summary>
        /// <param name="distance">The distance which the heuristic function must always return.</param>
        public static Goal<TBeliefSet>.HeuristicFunction Constant(float distance)
            => _ => new Heuristics { Distance = distance };

        /// <summary>
        /// Returns a heuristic function which always, at all times, and forever, returns a value indicating the state
        /// can be seen as completed.
        /// </summary>
        /// <returns>Said heuristic function.</returns>
        public static Goal<TBeliefSet>.HeuristicFunction Completed() => Constant(0f);

        /// <summary>
        /// Returns a heuristic function which always, at all times, and forever, returns a value indicating the state
        /// can be seen as NOT completed.
        /// </summary>
        /// <returns>Said heuristic function.</returns>
        public static Goal<TBeliefSet>.HeuristicFunction Uncompleted() => Constant(69_420f);
    }
}
