using Aplib.Core.Belief;
using Aplib.Core.Desire.Goals;

namespace Aplib.Core.Desire
{
    /// <inheritdoc />
    public class DesireSet<TBeliefSet> : IDesireSet<TBeliefSet>
        where TBeliefSet : IBeliefSet
    {
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
        public DesireSet(IGoalStructure<TBeliefSet> mainGoal) => _mainGoal = mainGoal;

        /// <inheritdoc />
        public IGoal GetCurrentGoal(TBeliefSet beliefSet)
        {
            return _mainGoal.GetCurrentGoal(beliefSet);
        }

        /// <inheritdoc />
        public void UpdateStatus(TBeliefSet beliefSet)
        {
            _mainGoal.UpdateStatus(beliefSet);
        }
    }
}
