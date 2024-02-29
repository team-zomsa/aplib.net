namespace Aplib.Core.Desire
{
    /// <summary>
    /// This is called a predicate, but really it represents the way to calculate the current state's heuristic values.
    /// </summary>
    /// <seealso cref="Heuristics"/>
    public interface IGoalPredicate
    {
        /// <summary>
        /// Invokes this predicate to determine the heuristics of the current state
        /// </summary>
        /// <returns>The heuristics of the tested state</returns>
        public Heuristics Test();
    }
}