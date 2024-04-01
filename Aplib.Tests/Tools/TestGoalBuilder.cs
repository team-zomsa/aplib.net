using Aplib.Core;
using Aplib.Core.Desire.Goals;
using Aplib.Core.Intent.Tactics;
using Moq;

namespace Aplib.Tests.Tools;

internal sealed class TestGoalBuilder
{
    private string _description = "\"A lie is just a good story that someone ruined with the truth.\" ~ Barney Stinson";
    private Goal.HeuristicFunction _heuristicFunction = CommonHeuristicFunctions.Constant(0);
    private string _name = "Such a good goal name";
    private Tactic _tactic = Mock.Of<Tactic>();

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


    public Goal Build() => new(_tactic, _heuristicFunction, metadata: new Metadata(_name, _description));
}
