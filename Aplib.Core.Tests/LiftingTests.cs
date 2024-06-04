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
        [new Metadata()],
        [new Metadata("Kaas", "saaK")],
        [new Metadata(name: "Kaas")],
        [new Metadata(description: "aap noot mies") ]
    };

    /// <summary>
    /// Given a query action lifted to a tactic,
    /// When compared to the latter, manually created to be trivially correct,
    /// Then the two should be equal, but the metadata name should differ.
    /// </summary>
    [Fact]
    public void ImplicitLifting_QueryActionLiftedToTactic_DoesNotDifferFromManualTactic()
    {
        // Arrange
        QueryAction<IBeliefSet, object> action = new((_, _) => { }, _ => 69);

        // Act
        Tactic<IBeliefSet> liftedTactic = action;
        Tactic<IBeliefSet> manualTactic = new PrimitiveTactic<IBeliefSet>(queryAction: action, _ => true);

        // Assert
        liftedTactic.GetAction(It.IsAny<IBeliefSet>())
            .Should().Be(manualTactic.GetAction(It.IsAny<IBeliefSet>()));
        // TODO assert guard
    }

    /// <summary>
    /// Given a query action lifted to a tactic,
    /// When compared to the latter, manually created to be trivially correct,
    /// Then the two should be equal, but the metadata name should differ.
    /// </summary>
    [Theory]
    [MemberData(nameof(MetadataMemberData))]
    public void ExplicitLifting_QueryActionLiftedToTactic_DoesNotDifferFromManualTactic(Metadata metadata)
    {
        // Arrange
        Action<IBeliefSet> action = It.IsAny<Action<IBeliefSet>>();

        // Act
        Tactic<IBeliefSet> liftedTactic = action.Lift(metadata);
        Tactic<IBeliefSet> manualTactic = new PrimitiveTactic<IBeliefSet>(metadata, action, _ => true);

        // Assert
        liftedTactic.Should().BeEquivalentTo(manualTactic);
    }

    /// <summary>
    /// Given a query action lifted to a tactic,
    /// When compared to the latter, manually created to be trivially correct,
    /// Then the two should be equal, but the metadata name should differ.
    /// </summary>
    [Fact]
    public void Lifting_IQueryableLiftedToTactic_DoesNotDifferFromManualTactic()
    {
        // Arrange
        Action<IBeliefSet> action = It.IsAny<Action<IBeliefSet>>();

        // Act
        Tactic<IBeliefSet> liftedTactic = action.Lift();
        Tactic<IBeliefSet> manualTactic = new PrimitiveTactic<IBeliefSet>(action, _ => true);

        // Assert
        liftedTactic.Should().BeEquivalentTo(manualTactic, config => config.Excluding(o => o.Metadata.Id));
    }

    /// <summary>
    /// Given an action lifted to a tactic,
    /// When compared to the latter, manually created to be trivially correct,
    /// Then the two should be equal, but the metadata name should differ.
    /// </summary>
    [Fact]
    public void ImplicitLifting_ActionLiftedToTactic_DoesNotDifferFromManualTactic()
    {
        // Arrange
        Action<IBeliefSet> action = It.IsAny<Action<IBeliefSet>>();

        // Act
        Tactic<IBeliefSet> liftedTactic = action;
        Tactic<IBeliefSet> manualTactic = new PrimitiveTactic<IBeliefSet>(action, _ => true);

        // Assert
        liftedTactic.GetAction(It.IsAny<IBeliefSet>())
            .Should().Be(manualTactic.GetAction(It.IsAny<IBeliefSet>()));
        // TODO assert guard
    }

    /// <summary>
    /// Given an action lifted to a tactic,
    /// When compared to the latter, manually created to be trivially correct,
    /// Then the two should be equal, but the metadata name should differ.
    /// </summary>
    [Theory]
    [MemberData(nameof(MetadataMemberData))]
    public void ExplicitLifting_ActionLiftedToTactic_DoesNotDifferFromManualTactic(Metadata metadata)
    {
        // Arrange
        Action<IBeliefSet> action = It.IsAny<Action<IBeliefSet>>();

        // Act
        Tactic<IBeliefSet> liftedTactic = action.Lift(metadata);
        Tactic<IBeliefSet> manualTactic = new PrimitiveTactic<IBeliefSet>(metadata, action, _ => true);

        // Assert
        liftedTactic.Should().BeEquivalentTo(manualTactic);
    }

    /// <summary>
    /// Given a goal lifted to a goal structure,
    /// When compared to the latter, manually created to be trivially correct,
    /// Then the two should be equal, but the metadata name should differ.
    /// </summary>
    [Fact]
    public void ImplicitLifting_GoalLiftedToGoalStructure_DoesNotDifferFromManualTactic()
    {
        // Arrange
        Goal<IBeliefSet> goal = new(It.IsAny<Tactic<IBeliefSet>>(), _ => true);

        // Act
        GoalStructure<IBeliefSet> liftedGoalStructure = goal;
        GoalStructure<IBeliefSet> manualGoalStructure = new PrimitiveGoalStructure<IBeliefSet>(goal);

        // Assert
        liftedGoalStructure.Should().BeEquivalentTo(manualGoalStructure, config => config
            .Excluding(o => o.Metadata.Id));
    }

    /// <summary>
    /// Given a goal lifted to a goal structure,
    /// When compared to the latter, manually created to be trivially correct,
    /// Then the two should be equal, but the metadata name should differ.
    /// </summary>
    [Theory]
    [MemberData(nameof(MetadataMemberData))]
    public void ExplicitLifting_GoalLiftedToGoalStructure_DoesNotDifferFromManualTactic(Metadata metadata)
    {
        // Arrange
        Goal<IBeliefSet> goal = It.IsAny<Goal<IBeliefSet>>();

        // Act
        GoalStructure<IBeliefSet> liftedGoalStructure = goal.Lift(metadata);
        GoalStructure<IBeliefSet> manualGoalStructure = new PrimitiveGoalStructure<IBeliefSet>(metadata, goal);

        // Assert
        liftedGoalStructure.Should().BeEquivalentTo(manualGoalStructure);
    }

    /// <summary>
    /// Given a goal structure lifted to a desire set,
    /// When compared to the latter, manually created to be trivially correct,
    /// Then the two should be equal, but the metadata name should differ.
    /// </summary>
    [Fact]
    public void ImplicitLifting_GoalStructureLiftedToDesireSet_DoesNotDifferFromManualTactic()
    {
        // Arrange
        GoalStructure<IBeliefSet> goalStructure = new PrimitiveGoalStructure<IBeliefSet>(It.IsAny<Goal<IBeliefSet>>());

        // Act
        DesireSet<IBeliefSet> liftedDesireSet = goalStructure;
        DesireSet<IBeliefSet> manualDesireSet = new(goalStructure.Metadata, goalStructure);

        // Assert
        liftedDesireSet.Should().BeEquivalentTo(manualDesireSet, config => config
            .Excluding(o => o.Metadata.Id));
    }

    /// <summary>
    /// Given a goal structure lifted to a desire set,
    /// When compared to the latter, manually created to be trivially correct,
    /// Then the two should be equal, but the metadata name should differ.
    /// </summary>
    [Theory]
    [MemberData(nameof(MetadataMemberData))]
    public void ExplicitLifting_GoalStructureLiftedToDesireSet_DoesNotDifferFromManualTactic(Metadata metadata)
    {
        // Arrange
        GoalStructure<IBeliefSet> goalStructure = It.IsAny<GoalStructure<IBeliefSet>>();

        // Act
        DesireSet<IBeliefSet> liftedDesireSet = goalStructure.Lift(metadata);
        DesireSet<IBeliefSet> manualDesireSet = new(metadata, goalStructure);

        // Assert
        liftedDesireSet.Should().BeEquivalentTo(manualDesireSet);
    }

    /// <summary>
    /// Given a goal lifted to a desire set,
    /// When compared to the latter, manually created to be trivially correct,
    /// Then the two should be equal, but the metadata name should differ.
    /// </summary>
    [Fact]
    public void ImplicitLifting_GoalLiftedToDesireSet_DoesNotDifferFromManualTactic()
    {
        // Arrange
        Goal<IBeliefSet> goal = new(It.IsAny<Tactic<IBeliefSet>>(), _ => true);

        // Act
        DesireSet<IBeliefSet> liftedDesireSet = goal;
        DesireSet<IBeliefSet> manualDesireSet = new(goal.Metadata, new PrimitiveGoalStructure<IBeliefSet>(goal));

        // Assert
        liftedDesireSet.Should().BeEquivalentTo(manualDesireSet, config => config
            .Excluding(o => o.Metadata.Id));
    }

    /// <summary>
    /// Given a goal lifted to a desire set,
    /// When compared to the latter, manually created to be trivially correct,
    /// Then the two should be equal, but the metadata name should differ.
    /// </summary>
    [Theory]
    [MemberData(nameof(MetadataMemberData))]
    public void ExplicitLifting_GoalLiftedToDesireSet_DoesNotDifferFromManualTactic(Metadata metadata)
    {
        // Arrange
        Goal<IBeliefSet> goal = It.IsAny<Goal<IBeliefSet>>();

        // Act
        DesireSet<IBeliefSet> liftedDesireSet = goal.Lift().Lift(metadata);
        DesireSet<IBeliefSet> manualDesireSet = new(metadata, new PrimitiveGoalStructure<IBeliefSet>(goal));

        // Assert
        liftedDesireSet.Should().BeEquivalentTo(manualDesireSet, config => config);
    }

    /// <summary>
    /// Given an undocumented action which is lifted to a tactic,
    /// When compared to the latter, manually created to be trivially correct,
    /// Then the two should be equal, but the metadata name should differ.
    /// </summary>
    [Fact]
    public void Lifting_UndocumentedActionToTactic_DoesNotDifferFromManualTactic()
    {
        Mock<IAction<IBeliefSet>> undocumentedAction = new();

        // Act
        Tactic<IBeliefSet> liftedTactic = undocumentedAction.Object.Lift();
        Tactic<IBeliefSet> manualTactic = new PrimitiveTactic<IBeliefSet>(undocumentedAction.Object, _ => true);

        // Assert
        liftedTactic.Should().BeEquivalentTo(manualTactic, config => config
            .Excluding(o => o.Metadata.Id));
    }

    /// <summary>
    /// Given an undocumented action which is lifted to a tactic,
    /// When compared to the latter, manually created to be trivially correct,
    /// Then the two should be equal, but the metadata name should differ.
    /// </summary>
    [Fact]
    public void Lifting_UndocumentedQueryActionToTactic_DoesNotDifferFromManualTactic()
    {
        Mock<IQueryable<IBeliefSet>> undocumentedQueryAction = new();

        // Act
        Tactic<IBeliefSet> liftedTactic = undocumentedQueryAction.Object.Lift();
        Tactic<IBeliefSet> manualTactic = new PrimitiveTactic<IBeliefSet>(undocumentedQueryAction.Object, _ => true);

        // Assert
        liftedTactic.Should().BeEquivalentTo(manualTactic, config => config
            .Excluding(o => o.Metadata.Id));
    }

    /// <summary>
    /// Given an undocumented goal which is lifted to a goal structure,
    /// When compared to the latter, manually created to be trivially correct,
    /// Then the two should be equal, but the metadata name should differ.
    /// </summary>
    [Fact]
    public void Lifting_UndocumentedGoalToGoalStructure_DoesNotDifferFromManualTactic()
    {
        // Arrange
        Mock<IGoal<IBeliefSet>> undocumentedGoal = new();

        // Act
        GoalStructure<IBeliefSet> liftedGoalStructure = undocumentedGoal.Object.Lift();
        GoalStructure<IBeliefSet> manualGoalStructure = new PrimitiveGoalStructure<IBeliefSet>(undocumentedGoal.Object);

        // Assert
        liftedGoalStructure.Should().BeEquivalentTo(manualGoalStructure, config => config
            .Excluding(o => o.Metadata.Id));
    }

    /// <summary>
    /// Given an undocumented goal lifted to a desire set,
    /// When compared to the latter, manually created to be trivially correct,
    /// Then the two should be equal, but the metadata name should differ.
    /// </summary>
    [Fact]
    public void Lifting_UndocumentedGoalLiftedToDesireSet_DoesNotDifferFromManualTactic()
    {
        // Arrange
        Mock<IGoal<IBeliefSet>> undocumentedGoal = new();

        // Act
        DesireSet<IBeliefSet> liftedDesireSet = undocumentedGoal.Object.Lift();
        DesireSet<IBeliefSet> manualDesireSet = new(new PrimitiveGoalStructure<IBeliefSet>(undocumentedGoal.Object));

        // Assert
        liftedDesireSet.Should().BeEquivalentTo(manualDesireSet, config => config
            .Excluding(o => o.Metadata.Id));
    }

    /// <summary>
    /// Given an undocumented goal structure which is lifted to a desire set,
    /// When compared to the latter, manually created to be trivially correct,
    /// Then the two should be equal, but the metadata name should differ.
    /// </summary>
    [Fact]
    public void Lifting_UndocumentedGoalStructureToDesireSet_DoesNotDifferFromManualTactic()
    {
        // Arrange
        Mock<IGoalStructure<IBeliefSet>> undocumentedGoalStructure = new();

        // Act
        DesireSet<IBeliefSet> liftedDesireSet = undocumentedGoalStructure.Object.Lift();
        DesireSet<IBeliefSet> manualDesireSet = new(undocumentedGoalStructure.Object);

        // Assert
        liftedDesireSet.Should().BeEquivalentTo(manualDesireSet, config => config
            .Excluding(o => o.Metadata.Id));
    }
}
