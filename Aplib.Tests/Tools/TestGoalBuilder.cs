using Aplib.Core.Desire;
using Aplib.Tests.Stubs.Desire;

namespace Aplib.Tests.Tools;

internal sealed class TestGoalBuilder
{
    private Tactic _tactic = new TacticStub(() => {});
    private Goal.HeuristicFunction _heuristicFunction = CommonGoalHeuristicFunctions.Constant(0);
    private string _name = "Such a good goal name";
    private string _description = "\"A lie is just a good story that someone ruined with the truth.\" ~ Barney Stinson";

    public TestGoalBuilder() { }

    public TestGoalBuilder WithHeuristicFunction(Goal.HeuristicFunction heuristicFunction)
    {
        _heuristicFunction = heuristicFunction;
        return this;
    }

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


    public Goal Build() => new (_tactic, _heuristicFunction, _name, _description);
}