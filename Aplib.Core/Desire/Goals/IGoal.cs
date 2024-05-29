using Aplib.Core.Belief.BeliefSets;
using Aplib.Core.Intent.Tactics;

namespace Aplib.Core.Desire.Goals
{
    /// <summary>
    /// Defines a goal that can be achieved by a <see cref="Tactic{TBeliefSet}" />.
    /// </summary>
    /// <typeparam name="TBeliefSet">The belief set of the agent.</typeparam>
    public interface IGoal<in TBeliefSet> : ICompletable
        where TBeliefSet : IBeliefSet
    {
        /// <summary>
        /// The <see cref="Tactic{TBeliefSet}" /> used to achieve this <see cref="Goal{TBeliefSet}" />, which is executed during every
        /// iteration of the BDI cycle.
        /// </summary>
        ITactic<TBeliefSet> Tactic { get; }

        /// <summary>
        /// Gets the <see cref="Heuristics" /> of the current state of the game.
        /// </summary>
        /// <remarks>If no heuristics have been calculated yet, they will be calculated first.</remarks>
        Heuristics DetermineCurrentHeuristics(TBeliefSet beliefSet);

        /// <summary>
        /// Tests whether the goal has been achieved, based on the <see cref="Goal{TBeliefSet}._heuristicFunction" /> and the
        /// <see cref="Goal{TBeliefSet}.DetermineCurrentHeuristics" />. When the distance of the heuristics is smaller than <see cref="Goal{TBeliefSet}._epsilon" />
        /// , the goal is considered to be completed.
        /// </summary>
        /// <returns>An enum representing whether the goal is complete and if so, with what result.</returns>
        /// <seealso cref="Goal{TBeliefSet}._epsilon" />
        CompletionStatus GetStatus(TBeliefSet beliefSet);
    }
}
