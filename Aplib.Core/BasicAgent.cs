namespace Aplib.Core
{
    public class BasicAgent : IAgent
    {
        public IState State { get; }
        public IEnvironment Environment { get; }
        public GoalStructure Goals { get; }

        public BasicAgent(IState state, IEnvironment environment, GoalStructure goals)
        {
            State = state;
            Environment = environment;
            Goals = goals;
        }

        public void Update()
        {
            Observation observation = Environment.Observe();
            State.Update(observation);

            Goal goal = Goals.NextGoal();
            goal.Iterate();
        }
    }
}