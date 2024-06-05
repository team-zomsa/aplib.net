using Aplib.Core.Belief.BeliefSets;

namespace Aplib.Core.Intent.Actions
{
    /// <summary>
    /// Represents an interface for executing queries on a belief set.
    /// </summary>
    /// <typeparam name="TBeliefSet">The type of the query object.</typeparam>
    public interface IQueryable<in TBeliefSet> : IAction<TBeliefSet>
        where TBeliefSet : IBeliefSet
    {
        /// <summary>
        /// Executes a query on the specified belief set.
        /// </summary>
        /// <param name="beliefSet">The belief set to query.</param>
        /// <returns>A boolean value indicating whether the query executed successfully or not.</returns>
        public bool Query(TBeliefSet beliefSet);
    }
}
