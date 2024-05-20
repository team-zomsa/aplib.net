using Aplib.Core.Belief.BeliefSets;
using Aplib.Core.Desire.Goals;
using Aplib.Core.Intent.Tactics;
using Moq;

namespace Aplib.Core.Tests.Tools;

internal sealed class TestGoalBuilder
{
    private string _description = "\"A lie is just a good story that someone ruined with the truth.\" ~ Barney Stinson";
    private Goal<IBeliefSet>.HeuristicFunction _heuristicFunction = CommonHeuristicFunctions<IBeliefSet>.Constant(0);
    private string _name = "Such a good goal name";
    private ITactic<IBeliefSet> _tactic = Mock.Of<ITactic<IBeliefSet>>();

    public TestGoalBuilder WithHeuristicFunction(Goal<IBeliefSet>.HeuristicFunction heuristicFunction)
    {
        _heuristicFunction = heuristicFunction;
        return this;
    }

    public TestGoalBuilder WithHeuristicFunction(System.Func<IBeliefSet, bool> heuristicFunction)
        => WithHeuristicFunction(CommonHeuristicFunctions<IBeliefSet>.Boolean(heuristicFunction));

    public TestGoalBuilder UseTactic(ITactic<IBeliefSet> tactic)
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


    public Goal<IBeliefSet> Build() => new(new Metadata(_name, _description), _tactic, _heuristicFunction);
}
