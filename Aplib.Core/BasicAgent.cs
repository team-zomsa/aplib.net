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

        public int UpdateTick()
        {
            Observation observation = Environment.Observe();
            State.Update(observation);

            // TODO Perform tactic

            // TODO Move a tick

            // TODO What if there is no budget?
            return --Budget;
        }
    }
}