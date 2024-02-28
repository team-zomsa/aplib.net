namespace Aplib.Core.Desire
{
	public class Goal
	{
		/// <summary>
		/// The goal is considered to be completed, when the distance of the
		/// <see cref="CurrentHeuristics"/> is below this value.
		/// </summary>
		private const float Epsilon = 0.005f;

		/// <summary>
		/// The <see cref="Heuristics"/> of the current state of the game.
		/// </summary>
		public Heuristics CurrentHeuristics { get; private set; }

		/// <summary>
		/// The <see cref="Tactic"/> used to achieve this <see cref="Goal"/>, which is executed during every iteration
		/// of the BDI cycle.
		/// </summary>
		/// <seealso cref="Iterate()"/>
		private readonly Tactic _tactic;
		/// <summary>
		/// The <see cref="IGoalPredicate"/> used to test whether this <see cref="Goal"/> has been completed.
		/// </summary>
		/// <seealso cref="IsCompleted()"/>
		private readonly IGoalPredicate _goalPredicate;

		// MetaData useful for debugging
		/// <summary>
		/// The name used to display the current goal during debugging, logging, or general overviews.
		/// </summary>
		public readonly string Name;
		/// <summary>
		/// The description used to describe the current goal during debugging, logging, or general overviews.
		/// </summary>
		public readonly string Description;

		/// <summary>
		/// A goal effectively combines a predicate with a tactic, and aims to meet the predicate by applying the tactic.
		/// Goals are combined in a <see cref="GoalStructure"/>, and are used to prepare tests or do the testing.
		/// </summary>
		/// <param name="tactic">The tactic used to approach this goal.</param>
		/// <param name="goalPredicate">The predicate which defines whether a goal is reached</param>
		/// <param name="name">The name of this goal, used to quickly display this goal in several contexts.</param>
		/// <param name="description">The description of this goal, used to explain this goal in several contexts.</param>
		/// <seealso cref="GoalStructure"/>
		public Goal(Tactic tactic, IGoalPredicate goalPredicate, string name, string description)
		{
			_tactic        = tactic;
			_goalPredicate = goalPredicate;
			Name           = name;
			Description    = description;

			CurrentHeuristics = _goalPredicate.Test(); // TODO is this the right time?
		}

		/// <summary>
		/// Performs the next steps needed to be taken to approach this goal. Effectively this means that one BDI
		/// cycle will be executed.
		/// </summary>
		public void Iterate()
		{
			_tactic.IterateBdiCycle();
		}

		/// <summary>
		/// Tests whether the goal has been achieved, bases on the <see cref="_goalPredicate"/> and the
		/// <see cref="CurrentHeuristics"/>. When the distance of the heuristics is smaller than <see cref="Epsilon"/>,
		/// the goal is considered to be achieved.
		/// </summary>
		/// <returns>A boolean representing whether the goal is considered to be achieved.</returns>
		/// <seealso cref="Epsilon"/>
		public bool IsCompleted()
		{
			CurrentHeuristics = _goalPredicate.Test();
			return CurrentHeuristics.Distance < Epsilon;
		}
	}
}