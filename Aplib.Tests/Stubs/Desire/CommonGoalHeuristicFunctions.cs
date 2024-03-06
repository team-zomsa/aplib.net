using Aplib.Core.Desire;

namespace Aplib.Tests.Stubs.Desire;

/// <summary>
/// Defines several commonly used implementations of the <see cref="Goal.HeuristicFunction"/>, to clean up tests.
/// </summary>
internal static class CommonGoalHeuristicFunctions
{
    /// <summary>
    /// A <see cref="Goal.HeuristicFunction"/> which always returns <see cref="Heuristics"/> with the same distance.
    /// </summary>
    /// <param name="distance">The distance which the heuristic function must always return.</param>
    public static Goal.HeuristicFunction Constant(float distance) => () => new Heuristics { Distance = distance };
}
