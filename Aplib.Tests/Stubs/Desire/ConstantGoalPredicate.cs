using Aplib.Core.Desire;

namespace Aplib.Tests.Stubs.Desire;

/// <summary>
/// A <see cref="IGoalPredicate"/> which always returns <see cref="Heuristics"/> with the same distance.
/// </summary>
/// <param name="distance">The distance which teh predicate must always return.</param>
public class ConstantGoalPredicate(float distance) : IGoalPredicate
{
	/// <inheritdoc />
	public Heuristics Test() => new() { Distance = distance };
}