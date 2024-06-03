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
    /// Given a query action lifted to a tactic,
    /// When compared to the latter, manually created to be trivially correct,
    /// Then the two should be equal, but the metadata name should differ.
    /// </summary>
    [Theory]
    [MemberData(nameof(MetadataMemberData))]
    public void Lifting_IQueryableLiftedToTactic_DoesNotDifferFromManualTactic(Metadata? metadata)
    {
        // Arrange
        System.Action<IBeliefSet, object> effect = (_, _) => { };
        System.Func<IBeliefSet, object> query = _ => 69;
        QueryAction<IBeliefSet, object> action = metadata is null ? new(effect, query) : new(metadata, effect, query);
        IQueryable<IBeliefSet> queryable = action;

        // Act
        Tactic<IBeliefSet> liftedTactic = queryable.Lift();
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
    /// Given a goal lifted to a desire set,
    /// When compared to the latter, manually created to be trivially correct,
    /// Then the two should be equal, but the metadata name should differ.
    /// </summary>
    [Theory]
    [MemberData(nameof(MetadataMemberData))]
    public void ImplicitLifting_GoalLiftedToDesireSet_DoesNotDifferFromManualTactic(Metadata? metadata)
    {
        // Arrange
        Goal<IBeliefSet> goal = metadata is null
            ? new(It.IsAny<Tactic<IBeliefSet>>(), _ => true)
            : new(metadata, It.IsAny<Tactic<IBeliefSet>>(), _ => true);

        // Act
        DesireSet<IBeliefSet> liftedDesireSet = goal;
        DesireSet<IBeliefSet> manualDesireSet = new(goal.Metadata, new PrimitiveGoalStructure<IBeliefSet>(goal));

        // Assert
        liftedDesireSet.Should().BeEquivalentTo(manualDesireSet, config => config
            .Excluding(o => o.Metadata.Id)
            .Excluding(o => o.Metadata.Name));
        liftedDesireSet.Metadata.Id.Should().NotBe(manualDesireSet.Metadata.Id);
        liftedDesireSet.Metadata.Name.Should().NotBe(manualDesireSet.Metadata.Name);
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
            .Excluding(o => o.Metadata.Id)
            .Excluding(o => o.Metadata.Name));
        liftedTactic.Metadata.Id.Should().NotBe(manualTactic.Metadata.Id);
        liftedTactic.Metadata.Name.Should().NotBe(manualTactic.Metadata.Name);
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
            .Excluding(o => o.Metadata.Id)
            .Excluding(o => o.Metadata.Name));
        liftedTactic.Metadata.Id.Should().NotBe(manualTactic.Metadata.Id);
        liftedTactic.Metadata.Name.Should().NotBe(manualTactic.Metadata.Name);
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
            .Excluding(o => o.Metadata.Id)
            .Excluding(o => o.Metadata.Name));
        liftedGoalStructure.Metadata.Id.Should().NotBe(manualGoalStructure.Metadata.Id);
        liftedGoalStructure.Metadata.Name.Should().NotBe(manualGoalStructure.Metadata.Name);
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
            .Excluding(o => o.Metadata.Id)
            .Excluding(o => o.Metadata.Name));
        liftedDesireSet.Metadata.Id.Should().NotBe(manualDesireSet.Metadata.Id);
        liftedDesireSet.Metadata.Name.Should().NotBe(manualDesireSet.Metadata.Name);
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
            .Excluding(o => o.Metadata.Id)
            .Excluding(o => o.Metadata.Name));
        liftedDesireSet.Metadata.Id.Should().NotBe(manualDesireSet.Metadata.Id);
        liftedDesireSet.Metadata.Name.Should().NotBe(manualDesireSet.Metadata.Name);
    }
}
