using Aplib.Core.Desire;

namespace Aplib.Tests.Stubs.Desire;

/// <summary>
/// A fake tactic, which is just a wrapper around the <see cref="Action"/> you define as argument.
/// </summary>
/// <param name="iteration">The method to be executed during iteration.</param>
public class TacticStub(Action iteration) : Tactic
{
    /// <inheritdoc />
    public override void IterateBdiCycle()
    {
        iteration.Invoke();
    }
}