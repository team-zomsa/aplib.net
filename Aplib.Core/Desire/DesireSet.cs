using Aplib.Core.Belief.BeliefSets;
using Aplib.Core.Desire.DesireSets;
using Aplib.Core.Desire.Goals;
using Aplib.Core.Desire.GoalStructures;

namespace Aplib.Core.Desire
{
    /// <inheritdoc cref="IDesireSet{TBeliefSet}"/>
    public class DesireSet<TBeliefSet> : IDesireSet<TBeliefSet>, IDocumented
        where TBeliefSet : IBeliefSet
    {
        /// <inheritdoc />
        public IMetadata Metadata { get; }

        /// <summary>
        /// Stores the main goal structure of the agent.
        /// </summary>
        private IGoalStructure<TBeliefSet> _mainGoal { get; }

        /// <inheritdoc />
        public CompletionStatus Status => _mainGoal.Status;

        /// <summary>
        /// Initializes a new instance of the <see cref="DesireSet{TBeliefSet}" /> class.
        /// </summary>
        /// <param name="mainGoal">The main goal structure that the agent needs to complete.</param>
        /// <param name="metadata">
        /// Metadata about this tactic, used to quickly display the tactic in several contexts.
        /// </param>
        public DesireSet(IGoalStructure<TBeliefSet> mainGoal, IMetadata? metadata = null)
        {
            _mainGoal = mainGoal;
            Metadata = metadata ?? new Metadata();
        }

        /// <inheritdoc />
        public IGoal<TBeliefSet> GetCurrentGoal(TBeliefSet beliefSet)
            => _mainGoal.GetCurrentGoal(beliefSet);

        /// <inheritdoc />
        public void UpdateStatus(TBeliefSet beliefSet)
            => _mainGoal.UpdateStatus(beliefSet);

        /// <summary>
        /// Implicitly lifts a goal into a desire set.
        /// </summary>
        /// <inheritdoc cref="LiftingExtensionMethods.Lift{TBeliefSet}(IGoal{TBeliefSet})" path="/param[@name='goal']"/>
        /// <returns>The most logically matching desire set, wrapping around <paramref name="goal"/>.</returns>
        public static implicit operator DesireSet<TBeliefSet>(Goal<TBeliefSet> goal) => goal.Lift();

        /// <summary>
        /// Implicitly lifts a goal structure a desire set.
        /// </summary>
        /// <inheritdoc cref="LiftingExtensionMethods.Lift{TBeliefSet}(IGoalStructure{TBeliefSet})" path="/param[@name='goalStructure']"/>
        /// <returns>The most logically matching desire set, wrapping around <paramref name="goalStructure"/>.</returns>
        public static implicit operator DesireSet<TBeliefSet>(GoalStructure<TBeliefSet> goalStructure) =>
            goalStructure.Lift();
    }
}
