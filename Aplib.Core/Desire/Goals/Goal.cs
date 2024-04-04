using Aplib.Core.Belief;
using Aplib.Core.Intent.Tactics;
using System;

namespace Aplib.Core.Desire.Goals
{
    /// <summary>
    /// A goal effectively combines a heuristic function with a tactic, and aims to meet the heuristic function by
    /// applying the tactic. Goals are combined in a <see cref="GoalStructure{TBeliefSet}" />, and are used to prepare tests
    /// or do
    /// the testing.
    /// </summary>
    /// <seealso cref="GoalStructure{TBeliefSet}" />
    /// <typeparam name="TBeliefSet">The belief set of the agent.</typeparam>
    public class Goal<TBeliefSet> : IGoal<TBeliefSet>
        where TBeliefSet : IBeliefSet
    {
        /// <summary>
        /// The abstract definition of what is means to test the Goal's heuristic function. Returns <see cref="Heuristics" />, as
        /// they represent how close we are to matching the heuristic function, and if the goal is completed.
        /// </summary>
        /// <seealso cref="Goal{TBeliefSet}.GetStatus" />
        public delegate Heuristics HeuristicFunction(TBeliefSet beliefSet);

        /// <summary>
        /// Gets the metadata of the goal.
        /// </summary>
        /// <remark>
        /// This metadata may be useful for debugging or logging.
        /// </remark>
        public Metadata Metadata { get; }

        /// <summary>
        /// The <see cref="Intent.Tactics.Tactic{TBeliefSet}" /> used to achieve this <see cref="Goal{TBeliefSet}" />, which is executed during every
        /// iteration of the BDI cycle.
        /// </summary>
        public ITactic<TBeliefSet> Tactic { get; }

        /// <inheritdoc />
        public CompletionStatus Status { get; protected set; }

        /// <summary>
        /// The goal is considered to be completed, when the distance of the <see cref="CurrentHeuristics" /> is below
        /// this value.
        /// </summary>
        protected double _epsilon { get; }
        /// <summary>
        /// The concrete implementation of this Goal's <see cref="HeuristicFunction" />. Used to test whether this goal is
        /// completed.
        /// </summary>
        /// <seealso cref="GetStatus" />
        protected HeuristicFunction _heuristicFunction;

        /// <summary>
        /// Creates a new goal which works with <see cref="Heuristics" />.
        /// </summary>
        /// <param name="tactic">The tactic used to approach this goal.</param>
        /// <param name="heuristicFunction">The heuristic function which defines whether a goal is reached</param>
        /// <param name="epsilon">
        /// The goal is considered to be completed, when the distance of the <see cref="CurrentHeuristics" /> is below
        /// this value.
        /// </param>
        /// <param name="metadata">
        /// Metadata about this goal, used to quickly display the goal in several contexts.
        /// </param>
        public Goal
        (
            ITactic<TBeliefSet> tactic,
            HeuristicFunction heuristicFunction,
            double epsilon = 0.005d,
            Metadata? metadata = null
        )
        {
            Tactic = tactic;
            _heuristicFunction = heuristicFunction;
            _epsilon = epsilon;
            Metadata = metadata ?? new Metadata();
        }

        /// <summary>
        /// Creates a new goal which works with boolean-based <see cref="Heuristics" />.
        /// </summary>
        /// <param name="tactic">The tactic used to approach this goal.</param>
        /// <param name="predicate">The heuristic function (or specifically predicate) which defines whether a goal is reached</param>
        /// <param name="epsilon">
        /// The goal is considered to be completed, when the distance of the <see cref="CurrentHeuristics" /> is below
        /// this value.
        /// </param>
        /// <param name="metadata">
        /// Metadata about this goal, used to quickly display the goal in several contexts.
        /// </param>
        public Goal(ITactic<TBeliefSet> tactic, Func<TBeliefSet, bool> predicate, double epsilon = 0.005d, Metadata? metadata = null)
        {
            Tactic = tactic;
            _heuristicFunction = CommonHeuristicFunctions<TBeliefSet>.Boolean(predicate);
            _epsilon = epsilon;
            Metadata = metadata ?? new Metadata();
        }

        /// <summary>
        /// Gets the <see cref="Heuristics" /> of the current state of the game.
        /// </summary>
        /// <remarks>If no heuristics have been calculated yet, they will be calculated first.</remarks>
        public virtual Heuristics CurrentHeuristics(TBeliefSet beliefSet) => _heuristicFunction.Invoke(beliefSet);

        /// <summary>
        /// Tests whether the goal has been achieved, bases on the <see cref="_heuristicFunction" /> and the
        /// <see cref="CurrentHeuristics" />. When the distance of the heuristics is smaller than <see cref="_epsilon" />,
        /// the goal is considered to be completed.
        /// </summary>
        /// <returns>An enum representing whether the goal is complete and if so, with what result.</returns>
        /// <seealso cref="_epsilon" />
        public virtual CompletionStatus GetStatus(TBeliefSet beliefSet)
        {
            Status = CurrentHeuristics(beliefSet).Distance < _epsilon
                ? CompletionStatus.Success
                : CompletionStatus.Unfinished;
            return Status;
        }
    }
}
