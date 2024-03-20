namespace Aplib.Core.Desire.Goals
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

        /// <summary>
        /// Creates a heuristic value representing just a boolean. The heuristic value is considered '0' or 'done' when
        /// the boolean is true. Non-zero otherwise.
        /// </summary>
        /// <param name="value">True if completed, False if not completed.</param>
        /// <returns></returns>
        public static Heuristics Boolean(bool value) => new() { Distance = value ? 0f : 1f };
    }
}
