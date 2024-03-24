using Aplib.Core.Desire.Goals;
using Aplib.Core.Intent.Tactics;
using Aplib.Tests.Stubs.Desire;
using Action = Aplib.Core.Intent.Actions.Action;

namespace Aplib.Tests.Tools;

internal sealed class TestGoalBuilder
{
    // ReSharper disable once StringLiteralTypo
    private Tactic _tactic = new TacticStub(new Action(() => { }, "a1"), "tictac");
    private Goal.HeuristicFunction _heuristicFunction = CommonHeuristicFunctions.Constant(0);
    private string _name = "Such a good goal name";
    private string _description = "\"A lie is just a good story that someone ruined with the truth.\" ~ Barney Stinson";

    public TestGoalBuilder() { }

    public TestGoalBuilder WithHeuristicFunction(Goal.HeuristicFunction heuristicFunction)
    {
        _heuristicFunction = heuristicFunction;
        return this;
    }
    public TestGoalBuilder WithHeuristicFunction(Func<bool> heuristicFunction)
        => WithHeuristicFunction(CommonHeuristicFunctions.Boolean(heuristicFunction));

    public TestGoalBuilder UseTactic(Tactic tactic)
    {
        _tactic = tactic;
        return this;
    }

    public TestGoalBuilder WithMetaData(string name, string description)
    {
        _name = name;
        _description = description;
        return this;
    }


    public Goal Build() => new(_tactic, _heuristicFunction, _name, _description);
}
