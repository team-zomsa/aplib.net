using Aplib.Core;
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
    /// <param name="metadata">
    /// Metadata about this tactic, used to quickly display the tactic in several contexts.
    /// </param>
    public TacticStub(Action iteration, Metadata? metadata = null)
        : base(metadata) => _iteration = iteration;

    /// <inheritdoc />
    public override Action? GetAction() => _iteration;
}
