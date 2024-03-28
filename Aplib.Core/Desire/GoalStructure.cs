using Aplib.Core.Belief;
using Aplib.Core.Desire.Goals;
using System.Collections.Generic;

namespace Aplib.Core.Desire
{
    public abstract class GoalStructure
    {
        protected IList<GoalStructure> _children;
        protected Goal? _currentGoal;
        public GoalStructureState GoalStructureState { get; set; } = GoalStructureState.Unfinished; // TODO

        protected GoalStructure(IList<GoalStructure> children)
        {
            _children = children;
        }

        public void Interrupt()
        {
            // Premature optimisation:
            // foreach (var processedChild in GetAllProcessedChildren())
            //     processedChild.Interrupt();
            foreach (GoalStructure child in _children)
                child.Interrupt();

            ProcessInterrupt();
        }

        public abstract Goal? DetermineCurrentGoal(IBeliefSet beliefSet);

        protected abstract void ProcessInterrupt();
    }
    
    public enum GoalStructureState { Unfinished, Success, Failure }
}
