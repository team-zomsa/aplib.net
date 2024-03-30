using Aplib.Core.Belief;
using Aplib.Core.Intent.Tactics;

namespace Aplib.Core.Desire.Goals
{
    public interface IGoal
    {
        /// <summary>
        /// The <see cref="Intent.Tactics.Tactic" /> used to achieve this <see cref="Goal" />, which is executed during every
        /// iteration
        /// of the BDI cycle.
        /// </summary>
        Tactic Tactic { get; }

        /// <summary>
        /// Gets the <see cref="Heuristics" /> of the current state of the game.
        /// </summary>
        /// <remarks>If no heuristics have been calculated yet, they will be calculated first.</remarks>
        Heuristics CurrentHeuristics(IBeliefSet beliefSet);

        /// <summary>
        /// Tests whether the goal has been achieved, bases on the <see cref="Goal._heuristicFunction" /> and the
        /// <see cref="Goal.CurrentHeuristics" />. When the distance of the heuristics is smaller than <see cref="Goal._epsilon" />
        /// ,
        /// the goal is considered to be completed.
        /// </summary>
        /// <returns>An enum representing whether the goal is complete and if so, with what result.</returns>
        /// <seealso cref="Goal._epsilon" />
        GoalState GetState(IBeliefSet beliefSet);
    }
}
