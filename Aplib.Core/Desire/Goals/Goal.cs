using Aplib.Core.Belief.BeliefSets;
using Aplib.Core.Desire.GoalStructures;
using Aplib.Core.Intent.Tactics;

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

        protected readonly System.Predicate<TBeliefSet> _failGuard;

        /// <summary>
        /// The concrete implementation of this Goal's <see cref="HeuristicFunction" />. Used to test whether this goal is
        /// completed.
        /// </summary>
        /// <seealso cref="UpdateStatus" />
        protected readonly HeuristicFunction _heuristicFunction;

        /// <summary>
        /// The abstract definition of what is means to test the Goal's heuristic function. Returns <see cref="Heuristics" />, as
        /// they represent how close we are to matching the heuristic function, and if the goal is completed.
        /// </summary>
        /// <seealso cref="Goal{TBeliefSet}.UpdateStatus" />
        public delegate Heuristics HeuristicFunction(TBeliefSet beliefSet);

        /// <inheritdoc />
        public IMetadata Metadata { get; }

        /// <summary>
        /// The <see cref="Intent.Tactics.Tactic{TBeliefSet}" /> used to achieve this <see cref="Goal{TBeliefSet}" />, which is
        /// executed during every
        /// iteration of the BDI cycle.
        /// </summary>
        public ITactic<TBeliefSet> Tactic { get; }

        /// <summary>
        /// Gets the completion status of the goal.
        /// This value may need to be updated first using the <see cref="UpdateStatus"/> method.
        /// </summary>
        /// <seealso cref="UpdateStatus"/>
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
            System.Predicate<TBeliefSet> failGuard,
            double epsilon = DefaultEpsilon
        )
        {
            Metadata = metadata;
            Tactic = tactic;
            _heuristicFunction = heuristicFunction;
            _failGuard = failGuard;
            _epsilon = epsilon;
        }

        /// <inheritdoc />
        public Goal
        (
            ITactic<TBeliefSet> tactic,
            HeuristicFunction heuristicFunction,
            System.Predicate<TBeliefSet> failGuard,
            double epsilon = DefaultEpsilon
        )
            : this(new Metadata(), tactic, heuristicFunction, failGuard, epsilon)
        {
        }

        /// <inheritdoc />
        public Goal
        (
            IMetadata metadata,
            ITactic<TBeliefSet> tactic,
            HeuristicFunction heuristicFunction,
            double epsilon = DefaultEpsilon
        ) : this(metadata, tactic, heuristicFunction, _ => false, epsilon)
        {
        }

        /// <inheritdoc />
        public Goal
        (
            ITactic<TBeliefSet> tactic,
            HeuristicFunction heuristicFunction,
            double epsilon = DefaultEpsilon
        )
            : this(tactic, heuristicFunction, _ => false, epsilon)
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
            System.Predicate<TBeliefSet> predicate,
            System.Predicate<TBeliefSet> failGuard,
            double epsilon = DefaultEpsilon
        )
            : this(metadata, tactic, CommonHeuristicFunctions<TBeliefSet>.Boolean(predicate), failGuard, epsilon)
        {
        }

        /// <inheritdoc />
        public Goal
        (
            ITactic<TBeliefSet> tactic,
            System.Predicate<TBeliefSet> predicate,
            System.Predicate<TBeliefSet> failGuard,
            double epsilon = DefaultEpsilon
        )
            : this(new Metadata(), tactic, predicate, failGuard, epsilon)
        {
        }

        /// <inheritdoc />
        public Goal
        (
            IMetadata metadata,
            ITactic<TBeliefSet> tactic,
            System.Predicate<TBeliefSet> predicate,
            double epsilon = DefaultEpsilon
        )
            : this(metadata, tactic, predicate, _ => false, epsilon)
        {
        }

        /// <inheritdoc />
        public Goal
        (
            ITactic<TBeliefSet> tactic,
            System.Predicate<TBeliefSet> predicate,
            double epsilon = DefaultEpsilon
        )
            : this(tactic, predicate, _ => false, epsilon)
        {
        }

        /// <summary>
        /// Gets the <see cref="Heuristics" /> of the current state of the game.
        /// </summary>
        /// <remarks>If no heuristics have been calculated yet, they will be calculated first.</remarks>
        public virtual Heuristics DetermineCurrentHeuristics(TBeliefSet beliefSet)
            => _heuristicFunction.Invoke(beliefSet);

        /// <summary>
        /// <para>Tests whether the goal has been achieved.</para>
        /// <para>
        /// This first checks the fail-guard of the goal, if it is true, the goal is considered to have failed.
        /// </para>
        /// <para>
        /// If the fail guard is false, this checks the heuristic function of the goal.
        /// When the distance of the heuristics is smaller than <see cref="_epsilon" />,
        /// the goal is considered to be completed.
        /// </para>
        /// <para>Otherwise, the goal is considered unfinished.</para>
        /// <para>Use <see cref="Status"/> to get the updated value.</para>
        /// </summary>
        /// <seealso cref="Status"/>
        public virtual void UpdateStatus(TBeliefSet beliefSet)
        {
            if (_failGuard(beliefSet))
                Status = CompletionStatus.Failure;
            else if (DetermineCurrentHeuristics(beliefSet).Distance < _epsilon)
                Status = CompletionStatus.Success;
            else
                Status = CompletionStatus.Unfinished;
        }
    }
}
