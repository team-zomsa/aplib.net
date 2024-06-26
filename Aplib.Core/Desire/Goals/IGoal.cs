// This program has been developed by students from the bachelor Computer Science at Utrecht
// University within the Software Project course.
// Copyright Utrecht University (Department of Information and Computing Sciences)

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
        /// Tests whether the goal has been achieved, based on the heuristic function of the goal.
        /// The new completion status can be accessed via the <see cref="ICompletable.Status"/> property.
        /// </summary>
        /// <seealso cref="ICompletable.Status"/>
        void UpdateStatus(TBeliefSet beliefSet);
    }
}
