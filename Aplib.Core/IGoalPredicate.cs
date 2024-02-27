namespace Aplib.Core
{
	public interface IGoalPredicate
	{
		/// <summary>
		/// Invokes this predicate to determine the heuristics of the current state
		/// </summary>
		/// <returns>The heuristics of the tested state</returns>
		public Heuristics Test();
	}
}