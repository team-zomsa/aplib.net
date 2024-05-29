using Aplib.Core.Belief;
using Aplib.Core.Desire;
using Aplib.Core.Desire.Goals;
using Aplib.Core.Intent.Actions;
using Aplib.Core.Intent.Tactics;

namespace Aplib.Core.Tests;

public class LiftingTests
{
    public void Test()
    {
        Action<BeliefSet> a = new(_ => { });
        Tactic<BeliefSet> t1 = a.Lift();
        Tactic<BeliefSet> t2 = a;

        Goal<BeliefSet> g = new(t1, CommonHeuristicFunctions<BeliefSet>.Completed());
        GoalStructure<BeliefSet> gs = new Goal<BeliefSet>(t1, CommonHeuristicFunctions<BeliefSet>.Completed());

        DesireSet<BeliefSet> d1 = g.Lift();
        DesireSet<BeliefSet> d2 = g;
        DesireSet<BeliefSet> d3 = gs.Lift();
        DesireSet<BeliefSet> d4 = gs;

        IGoal<BeliefSet> ginterface = new Goal<BeliefSet>(t1, CommonHeuristicFunctions<BeliefSet> .Completed());
        // DesireSet<BeliefSet> d5 = ginterface;  this would not work, for an interface cannot be implicitly converted
        DesireSet<BeliefSet> d6 = ginterface.Lift();
    }
}
