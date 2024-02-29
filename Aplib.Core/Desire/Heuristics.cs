namespace Aplib.Core.Desire
{
    /// <summary>
    /// Contains all information on how close the associated state is to its goal.
    /// Can be used to optimise search algorithms.
    /// </summary>
    public class Heuristics
    {
        /// <summary>
        /// The logical distance the current state is to its goal.
        /// </summary>
        public float Distance { get; set; }
    }
}