using Aplib.Core.Intent.Tactics;
using Action = Aplib.Core.Intent.Actions.Action;

namespace Aplib.Tests.Stubs.Desire;

/// <summary>
/// A fake tactic, which is just a wrapper around the <see cref="Action"/> you define as argument.
/// </summary>
internal class TacticStub : Tactic
{
    private readonly Action _iteration;

    /// <summary>
    ///     A fake tactic, which is just a wrapper around the <see cref="Action" /> you define as argument.
    /// </summary>
    /// <param name="iteration">The method to be executed during iteration.</param>
    /// <param name="name">The name of this Tactic, used to quickly display this goal in several contexts.</param>
    /// <param name="description">
    /// The description of this Tactic, used to explain this goal in several contexts.
    /// </param>
    public TacticStub(Action iteration, string name, string? description = null) : base(name, description)
        => _iteration = iteration;

    /// <inheritdoc />
    public override Action? GetAction() => _iteration;
}
