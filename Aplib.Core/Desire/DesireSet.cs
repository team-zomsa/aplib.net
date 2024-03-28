using Aplib.Core.Desire.Goals;

namespace Aplib.Core.Desire
{
    public class DesireSet
    {
        // Collections of goal structure

        // Stack of goal structures

        // Temporary, to make my life easier
        private readonly Goal _currentGoal;

        public DesireSet(Goal goal) => _currentGoal = goal;

        public Goal GetCurrentGoal() => _currentGoal;
    }
}
