using Aplib.Core.Belief.BeliefSets;
using Aplib.Core.Desire.GoalStructures;
using Aplib.Core.Intent.Tactics;
using System;

namespace Aplib.Core.Desire.Goals
{
    /// <summary>
    /// A goal effectively combines a heuristic function with a tactic, and aims to meet the heuristic function by
    /// applying the tactic. Goals are combined in a <see cref="GoalStructure{TBeliefSet}" />, and are used to
    /// prepare tests or do the testing.
    /// </summary>
    /// <seealso cref="GoalStructure{TBeliefSet}" />
    /// <typeparam name="TBeliefSet">The belief set of the agent.</typeparam>
    public class Goal<TBeliefSet> : IGoal<TBeliefSet>, IDocumented
        where TBeliefSet : IBeliefSet
    {
        /// <summary>
        /// The default value for the epsilon parameter in the Goal constructors.
        /// The epsilon parameter defines the threshold distance for a goal to be considered completed.
        /// </summary>
        protected const double DefaultEpsilon = 0.005d;

        /// <summary>
        /// The goal is considered to be completed, when the distance of the <see cref="DetermineCurrentHeuristics" /> is below
        /// this value.
        /// </summary>
        protected readonly double _epsilon;

        /// <summary>
        /// The concrete implementation of this Goal's <see cref="HeuristicFunction" />. Used to test whether this goal is
        /// completed.
        /// </summary>
        /// <seealso cref="GetStatus" />
        protected readonly HeuristicFunction _heuristicFunction;

        /// <summary>
        /// The abstract definition of what is means to test the Goal's heuristic function. Returns <see cref="Heuristics" />, as
        /// they represent how close we are to matching the heuristic function, and if the goal is completed.
        /// </summary>
        /// <seealso cref="Goal{TBeliefSet}.GetStatus" />
        public delegate Heuristics HeuristicFunction(TBeliefSet beliefSet);

        /// <inheritdoc />
        public IMetadata Metadata { get; }

        /// <summary>
        /// The <see cref="Intent.Tactics.Tactic{TBeliefSet}" /> used to achieve this <see cref="Goal{TBeliefSet}" />, which is
        /// executed during every
        /// iteration of the BDI cycle.
        /// </summary>
        public ITactic<TBeliefSet> Tactic { get; }

        /// <inheritdoc />
        public CompletionStatus Status { get; protected set; }

        /// <summary>
        /// Creates a new goal which works with <see cref="Heuristics" />.
        /// </summary>
        /// <param name="metadata">
        /// Metadata about this goal, used to quickly display the goal in several contexts.
        /// </param>
        /// <param name="tactic">The tactic used to approach this goal.</param>
        /// <param name="heuristicFunction">The heuristic function which defines whether a goal is reached.</param>
        /// <param name="epsilon">
        /// The goal is considered to be completed, when the distance of the <see cref="DetermineCurrentHeuristics" />
        /// is below this value.
        /// </param>
        public Goal
        (
            IMetadata metadata,
            ITactic<TBeliefSet> tactic,
            HeuristicFunction heuristicFunction,
            double epsilon = DefaultEpsilon
        )
        {
            Metadata = metadata;
            Tactic = tactic;
            _heuristicFunction = heuristicFunction;
            _epsilon = epsilon;
        }

        /// <inheritdoc
        ///     cref="Goal{TBeliefSet}(Aplib.Core.IMetadata,ITactic{TBeliefSet},Aplib.Core.Desire.Goals.Goal{TBeliefSet}.HeuristicFunction,double)" />
        public Goal(ITactic<TBeliefSet> tactic, HeuristicFunction heuristicFunction, double epsilon = DefaultEpsilon)
            : this(new Metadata(), tactic, heuristicFunction, epsilon)
        {
        }

        /// <summary>
        /// Creates a new goal which works with boolean-based <see cref="Heuristics" />.
        /// </summary>
        /// <param name="metadata">
        /// Metadata about this goal, used to quickly display the goal in several contexts.
        /// </param>
        /// <param name="tactic">The tactic used to approach this goal.</param>
        /// <param name="predicate">
        /// The heuristic function (or specifically predicate) which defines whether a goal is reached.
        /// </param>
        /// <param name="epsilon">
        /// The goal is considered to be completed, when the distance of the <see cref="DetermineCurrentHeuristics" />
        /// is below this value.
        /// </param>
        public Goal
        (
            IMetadata metadata,
            ITactic<TBeliefSet> tactic,
            Func<TBeliefSet, bool> predicate,
            double epsilon = DefaultEpsilon
        )
            : this(metadata, tactic, CommonHeuristicFunctions<TBeliefSet>.Boolean(predicate), epsilon)
        {
        }

        /// <inheritdoc cref="Goal{TBeliefSet}(Aplib.Core.IMetadata,ITactic{TBeliefSet},Func{TBeliefSet,bool},double)" />
        public Goal(ITactic<TBeliefSet> tactic, Func<TBeliefSet, bool> predicate, double epsilon = DefaultEpsilon)
            : this(new Metadata(), tactic, predicate, epsilon)
        {
        }

        /// <summary>
        /// Gets the <see cref="Heuristics" /> of the current state of the game.
        /// </summary>
        /// <remarks>If no heuristics have been calculated yet, they will be calculated first.</remarks>
        public virtual Heuristics DetermineCurrentHeuristics(TBeliefSet beliefSet)
            => _heuristicFunction.Invoke(beliefSet);

        /// <summary>
        /// Tests whether the goal has been achieved, bases on the <see cref="_heuristicFunction" /> and the
        /// <see cref="DetermineCurrentHeuristics" />. When the distance of the heuristics is smaller than <see cref="_epsilon" />,
        /// the goal is considered to be completed.
        /// </summary>
        /// <returns>An enum representing whether the goal is complete and if so, with what result.</returns>
        /// <seealso cref="_epsilon" />
        public virtual CompletionStatus GetStatus(TBeliefSet beliefSet)
        {
            Status = DetermineCurrentHeuristics(beliefSet).Distance < _epsilon
                ? CompletionStatus.Success
                : CompletionStatus.Unfinished;
            return Status;
        }
    }
}
