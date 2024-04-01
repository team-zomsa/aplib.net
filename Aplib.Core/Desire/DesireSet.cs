using Aplib.Core.Belief;
using Aplib.Core.Desire.Goals;
using System.Data;

namespace Aplib.Core.Desire
{
    public class DesireSet<TBeliefSet> : IDesireSet<TBeliefSet>
        where TBeliefSet : IBeliefSet
    {
        public GoalStructure<TBeliefSet> MainGoal { get; }
        
        /// <inheritdoc />
        public CompletionStatus Status => MainGoal.Status;
        
        public DesireSet(GoalStructure<TBeliefSet> mainGoal) => MainGoal = mainGoal;

        public IGoal GetCurrentGoal(TBeliefSet beliefSet)
        {
            MainGoal.UpdateStatus(beliefSet);
            return MainGoal.GetCurrentGoal(beliefSet);
        }
    }
}
