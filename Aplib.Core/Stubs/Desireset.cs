using Aplib.Core.Desire;

namespace Aplib.Core.Stubs
{
    internal class Desireset
    {
        private readonly GoalStructure _goalStructure;

        public Desireset(GoalStructure goalStructure) => _goalStructure = goalStructure;

        public Goal GetCurrentGoal() =>
            // Method intentionally left empty.
            _goalStructure.GetCurrentGoal();
    }
}
