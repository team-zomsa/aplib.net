namespace Aplib.Core
{
	public class Goal
	{
		private const float Epsilon = 0.005f;

		public Heuristics CurrentHeuristics { get; private set; }

		private readonly Tactic _tactic;
		private readonly IGoalPredicate _goalPredicate;

		// MetaData useful for debugging
		public readonly string Name;
		public readonly string Description;

		public Goal(Tactic tactic, IGoalPredicate goalPredicate, string name, string description)
		{
			_tactic        = tactic;
			_goalPredicate = goalPredicate;
			Name           = name;
			Description    = description;

			CurrentHeuristics = _goalPredicate.Test(); // TODO is this the right time?
		}

		public void Iterate()
		{
			_tactic.IterateBdiCycle();
		}

		public bool IsCompleted()
		{
			CurrentHeuristics = _goalPredicate.Test();
			return CurrentHeuristics.Distance < Epsilon;
		}
	}
}