using Aplib.Core.Belief.BeliefSets;

namespace Aplib.Core.Intent.Actions
{
    /// <summary>
    /// Represents an action that can be executed on a belief set.
    /// </summary>
    /// <typeparam name="TBeliefSet">The type of the belief set that the action uses.</typeparam>
    public interface IAction<in TBeliefSet>
        where TBeliefSet : IBeliefSet
    {
        /// <summary>
        /// Executes the action on the specified belief set.
        /// </summary>
        /// <param name="beliefSet">The belief set on which the action is executed.</param>
        void Execute(TBeliefSet beliefSet);
    }
}
