using Aplib.Core.Belief.BeliefSets;
using Aplib.Core.Desire.DesireSets;
using Aplib.Core.Desire.Goals;
using Aplib.Core.Desire.GoalStructures;
using Aplib.Core.Intent.Actions;
using Aplib.Core.Intent.Tactics;
using FluentAssertions;
using Moq;
using System.Collections.Generic;


namespace Aplib.Core.Tests
{
    public class ILoggableTests
    {
        private readonly Action<IBeliefSet> _action1;
        private readonly Action<IBeliefSet> _action2;
        private readonly PrimitiveTactic<IBeliefSet> _tactic1;
        private readonly PrimitiveTactic<IBeliefSet> _tactic2;
        private readonly Goal<IBeliefSet> _goal1;
        private readonly Goal<IBeliefSet> _goal2;

        /// <summary>
        /// Set up a DesireSet to use in the tests.
        /// </summary>
        public ILoggableTests()
        {
            _action1 = new(_ => { });
            _action2 = new(_ => { });
            _tactic1 = new(_action1, _ => true);
            _tactic2 = new(_action2, _ => true);
            _goal1 = new(_tactic1, _ => true);
            _goal2 = new(_tactic2, _ => true);
        }

        [Fact]
        public void PrimitiveTactic_WhenGettingTree_ReturnsCorrectTree()
        {
            // Arrange
            PrimitiveTactic<IBeliefSet> tactic = new(_action1, _ => true);

            LogNode expectedRoot = new(tactic, 0, [new LogNode(_action1, 1)]);
            
            // Act
            LogNode root = ((ILoggable)tactic).GetLogTree();

            // Assert
            root.Should().BeEquivalentTo(expectedRoot);
        }

        [Fact]
        public void AnyOfTactic_WhenGettingTree_ReturnsCorrectTree()
        {
            // Arrange
            AnyOfTactic<IBeliefSet> tactic = new(_tactic1, _tactic2);

            LogNode tactic1Node = new(_tactic1, 1, [new LogNode(_action1, 2)]);
            LogNode tactic2Node = new(_tactic2, 1, [new LogNode(_action2, 2)]);
            LogNode expectedRoot = new(tactic, 0, [tactic1Node, tactic2Node]);

            // Act
            LogNode root = ((ILoggable)tactic).GetLogTree();

            // Assert
            root.Should().BeEquivalentTo(expectedRoot);
        }

        [Fact]
        public void FirstOfTactic_WhenGettingTree_ReturnsCorrectTree()
        {
            // Arrange
            FirstOfTactic<IBeliefSet> tactic = new(_tactic1, _tactic2);

            LogNode tactic1Node = new(_tactic1, 1, [new LogNode(_action1, 2)]);
            LogNode tactic2Node = new(_tactic2, 1, [new LogNode(_action2, 2)]);
            LogNode expectedRoot = new(tactic, 0, [tactic1Node, tactic2Node]);

            // Act
            LogNode root = ((ILoggable)tactic).GetLogTree();

            // Assert
            root.Should().BeEquivalentTo(expectedRoot);
        }

        [Fact]
        public void Goal_WhenGettingTree_ReturnsCorrectTree()
        {
            // Arrange
            Action<IBeliefSet> action = new(_ => { });
            PrimitiveTactic<IBeliefSet> tactic = new(action, _ => true);
            Goal<IBeliefSet> goal = new(tactic, _ => true);

            LogNode tacticNode = new(tactic, 1, [new LogNode(action, 2)]);
            LogNode expectedRoot = new(goal, 0, [tacticNode]);

            // Act
            LogNode root = ((ILoggable)goal).GetLogTree();

            // Assert
            root.Should().BeEquivalentTo(expectedRoot);
        }

        [Fact]
        public void PrimitiveGoalStructure_WhenGettingTree_ReturnsCorrectTree()
        {
            // Arrange
            Goal<IBeliefSet> goal = new(_tactic1, _ => true);
            PrimitiveGoalStructure<IBeliefSet> goalStructure = new(goal);

            LogNode goalNode = new(goal, 1, [new LogNode(_tactic1, 2, [new LogNode(_action1, 3)])]);
            LogNode expectedRoot = new(goalStructure, 0, [goalNode]);

            // Act
            LogNode root = ((ILoggable)goalStructure).GetLogTree();

            // Assert
            root.Should().BeEquivalentTo(expectedRoot);
        }

        [Fact]
        public void SequentialGoalStructure_WhenGettingTree_ReturnsCorrectTree()
        {
            // Arrange
            PrimitiveGoalStructure<IBeliefSet> primitiveStructure1 = new(_goal1);
            PrimitiveGoalStructure<IBeliefSet> primitiveStructure2 = new(_goal2);
            SequentialGoalStructure<IBeliefSet> seqStructure = new(primitiveStructure1, primitiveStructure2);

            LogNode goalNode1 = new(_goal1, 2, [new LogNode(_tactic1, 3, [new LogNode(_action1, 4)])]);
            LogNode goalStructureNode1 = new(primitiveStructure1, 1, [goalNode1]);
            LogNode goalNode2 = new(_goal2, 2, [new LogNode(_tactic2, 3, [new LogNode(_action2, 4)])]);
            LogNode goalStructureNode2 = new(primitiveStructure2, 1, [goalNode2]);
            LogNode expectedRoot = new(seqStructure, 0, [goalStructureNode1, goalStructureNode2]);

            // Act
            LogNode root = ((ILoggable)seqStructure).GetLogTree();

            // Assert
            root.Should().BeEquivalentTo(expectedRoot);
        }

        [Fact]
        public void DesireSet_WhenGettingTree_ReturnsCorrectTree()
        {
            // Arrange
            Goal<IBeliefSet> goal = new(_tactic1, _ => true);
            PrimitiveGoalStructure<IBeliefSet> goalStructure = new(goal);
            DesireSet<IBeliefSet> desireSet = new(goalStructure);

            LogNode goalNode = new(goal, 2, [new LogNode(_tactic1, 3, [new LogNode(_action1, 4)])]);
            LogNode goalStructureNode = new(goalStructure, 1, [goalNode]);
            LogNode expectedRoot = new(desireSet, 0, [goalStructureNode]);

            // Act
            LogNode root = ((ILoggable)desireSet).GetLogTree();

            // Assert
            root.Should().BeEquivalentTo(expectedRoot);
        }
    }
}