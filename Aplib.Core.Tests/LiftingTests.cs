using Aplib.Core.Belief.BeliefSets;
using Aplib.Core.Desire.DesireSets;
using Aplib.Core.Desire.Goals;
using Aplib.Core.Desire.GoalStructures;
using Aplib.Core.Intent.Actions;
using Aplib.Core.Intent.Tactics;
using FluentAssertions;
using Moq;
using System.Collections.Generic;

namespace Aplib.Core.Tests;

/// <summary>
/// Contains all the tests related to lifting
/// </summary>
public class LiftingTests
{
    public static IEnumerable<object[]> MetadataMemberData => new object[][]
    {
        [null!],
        [new Metadata("Kaas", "saaK")],
        [new Metadata(name: "Kaas")],
        [new Metadata(description: "aap noot mies") ]
    };

    [Fact]
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

        IGoal<BeliefSet> ginterface = new Goal<BeliefSet>(t1, CommonHeuristicFunctions<BeliefSet>.Completed());
        // DesireSet<BeliefSet> d5 = ginterface;  this would not work, for an interface cannot be implicitly converted
        DesireSet<BeliefSet> d6 = ginterface.Lift();
    }

    /// <summary>
    /// Given a query action lifted to a tactic,
    /// When compared to the latter, manually created to be trivially correct,
    /// Then the two should be equal, but the metadata name should differ.
    /// </summary>
    [Theory]
    [MemberData(nameof(MetadataMemberData))]
    public void ImplicitLifting_QueryActionLiftedToTactic_DoesNotDifferFromManualTactic(Metadata? metadata)
    {
        // Arrange
        System.Action<IBeliefSet, object> effect = (_, _) => { };
        System.Func<IBeliefSet, object> query = _ => 69;
        QueryAction<IBeliefSet, object> action = metadata is null ? new(effect, query) : new(metadata, effect, query);

        // Act
        Tactic<IBeliefSet> liftedTactic = action;
        Tactic<IBeliefSet> manualTactic = new PrimitiveTactic<IBeliefSet>(action.Metadata, action, _ => true);

        // Assert
        liftedTactic.Should().BeEquivalentTo(manualTactic, config => config
            .Excluding(o => o.Metadata.Id)
            .Excluding(o => o.Metadata.Name));
        liftedTactic.Metadata.Id.Should().NotBe(manualTactic.Metadata.Id);
        liftedTactic.Metadata.Name.Should().NotBe(manualTactic.Metadata.Name);
    }

    /// <summary>
    /// Given an action lifted to a tactic,
    /// When compared to the latter, manually created to be trivially correct,
    /// Then the two should be equal, but the metadata name should differ.
    /// </summary>
    [Theory]
    [MemberData(nameof(MetadataMemberData))]
    public void ImplicitLifting_ActionLiftedToTactic_DoesNotDifferFromManualTactic(Metadata? metadata)
    {
        // Arrange
        System.Action<IBeliefSet> effect = _ => { };
        Action<IBeliefSet> action = metadata is null ? new(effect) : new(metadata, effect);

        // Act
        Tactic<IBeliefSet> liftedTactic = action;
        Tactic<IBeliefSet> manualTactic = new PrimitiveTactic<IBeliefSet>(action.Metadata, action, _ => true);

        // Assert
        liftedTactic.Should().BeEquivalentTo(manualTactic, config => config
            .Excluding(o => o.Metadata.Id)
            .Excluding(o => o.Metadata.Name));
        liftedTactic.Metadata.Id.Should().NotBe(manualTactic.Metadata.Id);
        liftedTactic.Metadata.Name.Should().NotBe(manualTactic.Metadata.Name);
    }

    /// <summary>
    /// Given a goal lifted to a goal structure,
    /// When compared to the latter, manually created to be trivially correct,
    /// Then the two should be equal, but the metadata name should differ.
    /// </summary>
    [Theory]
    [MemberData(nameof(MetadataMemberData))]
    public void ImplicitLifting_GoalLiftedToGoalStructure_DoesNotDifferFromManualTactic(Metadata? metadata)
    {
        // Arrange
        Goal<IBeliefSet> goal = metadata is null
            ? new(It.IsAny<Tactic<IBeliefSet>>(), _ => true)
            : new(metadata, It.IsAny<Tactic<IBeliefSet>>(), _ => true);

        // Act
        GoalStructure<IBeliefSet> liftedGoalStructure = goal;
        GoalStructure<IBeliefSet> manualGoalStructure = new PrimitiveGoalStructure<IBeliefSet>(goal.Metadata, goal);

        // Assert
        liftedGoalStructure.Should().BeEquivalentTo(manualGoalStructure, config => config
            .Excluding(o => o.Metadata.Id)
            .Excluding(o => o.Metadata.Name));
        liftedGoalStructure.Metadata.Id.Should().NotBe(manualGoalStructure.Metadata.Id);
        liftedGoalStructure.Metadata.Name.Should().NotBe(manualGoalStructure.Metadata.Name);
    }

    /// <summary>
    /// Given a goal structure lifted to a desire set,
    /// When compared to the latter, manually created to be trivially correct,
    /// Then the two should be equal, but the metadata name should differ.
    /// </summary>
    [Theory]
    [MemberData(nameof(MetadataMemberData))]
    public void ImplicitLifting_GoalStructureLiftedToDesireSet_DoesNotDifferFromManualTactic(Metadata? metadata)
    {
        // Arrange
        GoalStructure<IBeliefSet> goalStructure = metadata is null
            ? new PrimitiveGoalStructure<IBeliefSet>(It.IsAny<Goal<IBeliefSet>>())
            : new PrimitiveGoalStructure<IBeliefSet>(metadata, It.IsAny<Goal<IBeliefSet>>());

        // Act
        DesireSet<IBeliefSet> liftedDesireSet = goalStructure;
        DesireSet<IBeliefSet> manualDesireSet = new(goalStructure.Metadata, goalStructure);

        // Assert
        liftedDesireSet.Should().BeEquivalentTo(manualDesireSet, config => config
            .Excluding(o => o.Metadata.Id)
            .Excluding(o => o.Metadata.Name));
        liftedDesireSet.Metadata.Id.Should().NotBe(manualDesireSet.Metadata.Id);
        liftedDesireSet.Metadata.Name.Should().NotBe(manualDesireSet.Metadata.Name);
    }

    /// <summary>
    /// Given a query action stored as an IAction
    /// When the query action is lifted to a tactic
    /// Then the resulting tactic should have been created with the derived query action
    /// </summary>
    [Theory]
    [MemberData(nameof(MetadataMemberData))]
    public void ImplicitLifting_WhenQueryActionIsLiftedAsIAction_ItIsLiftedAsQueryAction(Metadata? metadata)
    {
        // Arrange
        System.Action<IBeliefSet, object> effect = (_, _) => { };
        System.Func<IBeliefSet, object> query = _ => 69;
        QueryAction<IBeliefSet, object> action = metadata is null
            ? new QueryAction<IBeliefSet, object>(effect, query)
            : new QueryAction<IBeliefSet, object>(metadata, effect, query);

        // Act
        Tactic<IBeliefSet> liftedTactic = ((IAction<IBeliefSet>)action).Lift();
        Tactic<IBeliefSet> manualTactic = new PrimitiveTactic<IBeliefSet>(action.Metadata, queryAction: action);

        // Assert
        liftedTactic.Should().BeEquivalentTo(manualTactic, config => config
            .Excluding(o => o.Metadata.Id)
            .Excluding(o => o.Metadata.Name));
        liftedTactic.Metadata.Id.Should().NotBe(manualTactic.Metadata.Id);
        liftedTactic.Metadata.Name.Should().NotBe(manualTactic.Metadata.Name);
    }
}
