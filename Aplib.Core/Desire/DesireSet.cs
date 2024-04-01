using Aplib.Core.Belief;
using Aplib.Core.Desire.Goals;

namespace Aplib.Core.Desire
{
    /// <inheritdoc />
    public class DesireSet<TBeliefSet> : IDesireSet<TBeliefSet>
        where TBeliefSet : IBeliefSet
    {
        /// <inheritdoc />
        public GoalStructure<TBeliefSet> MainGoal { get; }
        
        /// <inheritdoc />
        public CompletionStatus Status => MainGoal.Status;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="DesireSet{TBeliefSet}" /> class.
        /// </summary>
        /// <param name="mainGoal">The main goal structure that the agent needs to complete.</param>
        public DesireSet(GoalStructure<TBeliefSet> mainGoal) => MainGoal = mainGoal;

        /// <inheritdoc />
        public IGoal GetCurrentGoal(TBeliefSet beliefSet)
        {
            MainGoal.UpdateStatus(beliefSet);
            return MainGoal.GetCurrentGoal(beliefSet);
        }
    }
}
