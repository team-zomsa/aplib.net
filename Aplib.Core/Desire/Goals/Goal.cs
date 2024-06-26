// This program has been developed by students from the bachelor Computer Science at Utrecht
// University within the Software Project course.
// Copyright Utrecht University (Department of Information and Computing Sciences)

using Aplib.Core.Belief.BeliefSets;
using Aplib.Core.Desire.GoalStructures;
using Aplib.Core.Intent.Tactics;
using Aplib.Core.Logging;
using System.Collections.Generic;
using System.Linq;
using static Aplib.Core.CompletionStatus;

namespace Aplib.Core.Desire.Goals
{
    /// <summary>
    /// A goal effectively combines a heuristic function with a tactic, and aims to meet the heuristic function by
    /// applying the tactic. Goals are combined in a <see cref="GoalStructure{TBeliefSet}" />, and are used to
    /// prepare tests or do the testing.
    /// </summary>
    /// <seealso cref="GoalStructure{TBeliefSet}" />
    /// <typeparam name="TBeliefSet">The belief set of the agent.</typeparam>
    public class Goal<TBeliefSet> : IGoal<TBeliefSet>, ILoggable
        where TBeliefSet : IBeliefSet
    {
        /// <inheritdoc />
        public IMetadata Metadata { get; }

        /// <summary>
        /// Gets the completion status of the goal.
        /// This value may need to be updated first using the <see cref="UpdateStatus"/> method.
        /// </summary>
        /// <seealso cref="UpdateStatus"/>
        public CompletionStatus Status { get; protected set; }

        /// <summary>
        /// The <see cref="Tactic{TBeliefSet}" /> used to achieve this <see cref="Goal{TBeliefSet}" />.
        /// It is executed once in every iteration of the BDI cycle while this goal is the active goal of the agent.
        /// </summary>
        public ITactic<TBeliefSet> Tactic { get; }

        /// <summary>
        /// An (optional) fail-guard for the goal's completion status.
        /// The fail-guard predicate is a condition that, when true, indicates that the goal has failed.
        /// </summary>
        protected internal readonly System.Predicate<TBeliefSet> _failGuard;

        /// <summary>
        /// A predicate that determines whether the goal has succeeded.
        /// Intuitively, the predicate is the goal itself.
        /// </summary>
        protected internal readonly System.Predicate<TBeliefSet> _predicate;

        /// <summary>
        /// Initializes a new goal from a given tactic and a success predicate, and an optional fail-guard.
        /// </summary>
        /// <param name="metadata">
        /// Metadata about this goal, used to quickly display the goal in several contexts.
        /// If omitted, default metadata will be generated.
        /// </param>
        /// <param name="tactic">The tactic used to approach this goal.</param>
        /// <param name="predicate">A predicate that determines when the goal has succeeded.</param>
        /// <param name="failGuard">
        /// A predicate that determines when the goal has failed.
        /// If the fail-guard is true,
        /// but the success predicate is also satisfied, the success predicate takes precedence.
        /// If omitted, the goal will never fail.
        /// </param>
        public Goal
        (
            IMetadata metadata,
            ITactic<TBeliefSet> tactic,
            System.Predicate<TBeliefSet> predicate,
            System.Predicate<TBeliefSet> failGuard
        )
        {
            Metadata = metadata;
            Tactic = tactic;
            _predicate = predicate;
            _failGuard = failGuard;
        }

        /// <inheritdoc />
        public Goal
        (
            ITactic<TBeliefSet> tactic,
            System.Predicate<TBeliefSet> predicate,
            System.Predicate<TBeliefSet> failGuard
        )
            : this(new Metadata(), tactic, predicate, failGuard)
        {
        }

        /// <inheritdoc />
        public Goal
        (
            IMetadata metadata,
            ITactic<TBeliefSet> tactic,
            System.Predicate<TBeliefSet> predicate
        )
            : this(metadata, tactic, predicate, _ => false)
        {
        }

        /// <inheritdoc />
        public Goal
        (
            ITactic<TBeliefSet> tactic,
            System.Predicate<TBeliefSet> predicate
        )
            : this(new Metadata(), tactic, predicate, _ => false)
        {
        }

        /// <summary>
        /// <para>Checks whether the goal has been achieved and stores the result in <see cref="Status"/>.</para>
        /// <para>
        /// If the predicate of the goal is satisfied, the goal is considered to have succeeded.
        /// If the fail-guard is satisfied, the goal is considered to have failed.
        /// If both are satisfied, the success predicate takes precedence.
        /// If neither are satisfied, the goal is considered unfinished.
        /// The table below summarizes the possible outcomes:
        /// <list type="table">
        ///     <listheader>
        ///         <term>Predicate</term>
        ///         <term>Fail guard</term>
        ///         <term>Result</term>
        ///     </listheader>
        ///     <item>
        ///         <description><c>true</c></description>
        ///         <description><c>false</c></description>
        ///         <description><see cref="Success"/></description>
        ///     </item>
        ///     <item>
        ///         <description><c>true</c></description>
        ///         <description><c>true</c></description>
        ///         <description><see cref="Success"/></description>
        ///     </item>
        ///     <item>
        ///         <description><c>false</c></description>
        ///         <description><c>true</c></description>
        ///         <description><see cref="Failure"/></description>
        ///     </item>
        ///     <item>
        ///         <description><c>false</c></description>
        ///         <description><c>false</c></description>
        ///         <description><see cref="Unfinished"/></description>
        ///     </item>
        /// </list>
        /// </para>
        /// <remarks>Use <see cref="Status"/> to get the updated value.</remarks>
        /// </summary>
        /// <param name="beliefSet">The belief set of the agent.</param>
        public virtual void UpdateStatus(TBeliefSet beliefSet)
        {
            if (_predicate(beliefSet))
                Status = Success;
            else if (_failGuard(beliefSet))
                Status = Failure;
            else
                Status = Unfinished;
        }

        /// <inheritdoc />
        public IEnumerable<ILoggable> GetLogChildren() =>
            Tactic is ILoggable tactic ? new[] { tactic } : Enumerable.Empty<ILoggable>();
    }
}
