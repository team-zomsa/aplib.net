namespace Aplib.Core
{
    public interface IAgent
    {
        public IState State { get; }
        public IEnvironment Environment { get; }
        public GoalStructure Goals { get; }
        public int Budget { get; }

        /// <summary>Update the world and perform an action.</summary>
        /// <returns>The remaining budget.</returns>
        public int UpdateTick();
    }

    // Temporary type definitions
    public interface IEnvironment
    {
        public Observation Observe();
    }

    public class Observation
    {
    }

    public class GoalStructure
    {
    }
}