using Aplib.Core.Intent.Tactics;

namespace Aplib.Tests.Stubs.Desire;

/// <summary>
/// A fake tactic, which is just a wrapper around the <see cref="Action"/> you define as argument.
/// </summary>
/// <param name="iteration">The method to be executed during iteration.</param>
internal class TacticStub(Action iteration) : Tactic
{
    /// <inheritdoc />
    public override List<PrimitiveTactic> GetFirstEnabledTactics() => new List<PrimitiveTactic>(
        new PrimitiveTactic[]{new PrimitiveTactic(new Aplib.Core.Intent.Actions.Action(iteration))});
}
