using Aplib.Core.Belief;
using Aplib.Core.Desire.Goals;
using Aplib.Core.Desire.GoalStructures;

namespace Aplib.Core.Desire
{
    /// <inheritdoc />
    public class DesireSet<TBeliefSet> : IDesireSet<TBeliefSet>
        where TBeliefSet : IBeliefSet
    {
        /// <summary>
        /// Stores the main goal structure of the agent.
        /// </summary>
        private readonly IGoalStructure<TBeliefSet> _mainGoal;

        /// <inheritdoc />
        public CompletionStatus Status => _mainGoal.Status;

        /// <summary>
        /// Initializes a new instance of the <see cref="DesireSet{TBeliefSet}" /> class.
        /// </summary>
        /// <param name="mainGoal">The main goal structure that the agent needs to complete.</param>
        public DesireSet(IGoalStructure<TBeliefSet> mainGoal)
            => _mainGoal = mainGoal;

        /// <inheritdoc />
        public IGoal<TBeliefSet> GetCurrentGoal(TBeliefSet beliefSet)
            => _mainGoal.GetCurrentGoal(beliefSet);

        /// <inheritdoc />
        public void UpdateStatus(TBeliefSet beliefSet)
            => _mainGoal.UpdateStatus(beliefSet);
    }
}
