namespace Aplib.Core
{
    public class BasicAgent : IAgent
    {
        public IState State { get; }
        public IEnvironment Environment { get; }
        public GoalStructure Goals { get; }
        public int Budget { get; private set; }

        public BasicAgent(IState state, IEnvironment environment, GoalStructure goals, int budget)
        {
            State = state;
            Environment = environment;
            Goals = goals;
            Budget = budget;
        }

        public int Update()
        {
            Observation observation = Environment.Observe();
            State.Update(observation);

            Goal goal = Goals.NextGoal();
            goal.Iterate();

            // TODO What if there is no budget?
            return --Budget;
        }
    }
}