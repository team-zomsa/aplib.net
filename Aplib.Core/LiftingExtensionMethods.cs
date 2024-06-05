using Aplib.Core.Belief.BeliefSets;
using Aplib.Core.Desire.DesireSets;
using Aplib.Core.Desire.Goals;
using Aplib.Core.Desire.GoalStructures;
using Aplib.Core.Intent.Actions;
using Aplib.Core.Intent.Tactics;

namespace Aplib.Core
{
    /// <summary>
    /// Contains extension methods for lifting BDI cycle components into higher-order components.
    /// </summary>
    public static class LiftingExtensionMethods
    {
        /// <summary>
        /// Wraps a normal action into a tactic.
        /// </summary>
        /// <returns> A primitive tactic, whose guard always returns true. </returns>
        /// <param name="action">
        /// The action which on its own can function as a tactic. Meaning, the tactic consists of just a single action.
        /// </param>
        /// <param name="metadata">Optional metadata to be assigned to the tactic.</param>
        public static PrimitiveTactic<TBeliefSet> Lift<TBeliefSet>(this IAction<TBeliefSet> action, IMetadata metadata)
            where TBeliefSet : IBeliefSet => new(metadata, action: action);

        /// <inheritdoc cref="Lift{TBeliefSet}(IAction{TBeliefSet},IMetadata)" />
        public static PrimitiveTactic<TBeliefSet> Lift<TBeliefSet>(this IAction<TBeliefSet> action)
            where TBeliefSet : IBeliefSet => action.Lift(new Metadata());

        /// <summary>
        /// Wraps a queryable action into a tactic.
        /// </summary>
        /// <returns> A primitive tactic, whose guard always returns true. </returns>
        /// <param name="action">
        /// The action which on its own can function as a tactic. Meaning, the tactic consists of just a single action.
        /// </param>
        /// <param name="metadata">Optional metadata to be assigned to the tactic.</param>
        public static PrimitiveTactic<TBeliefSet> Lift<TBeliefSet>(this IQueryable<TBeliefSet> action, IMetadata metadata)
            where TBeliefSet : IBeliefSet => new(metadata, queryAction: action);


        /// <inheritdoc cref="Lift{TBeliefSet}(IQueryable{TBeliefSet},IMetadata)" />
        public static PrimitiveTactic<TBeliefSet> Lift<TBeliefSet>(this IQueryable<TBeliefSet> action)
            where TBeliefSet : IBeliefSet => action.Lift(new Metadata());

        /// <summary>
        /// Wraps a goal into a goal structure.
        /// </summary>
        /// <returns> A primitive goal structure. </returns>
        /// <param name="goal">
        /// The goal which on its own can function as a goal structure. Meaning, the goal structure consists of just a
        /// single goal.
        /// </param>
        /// <param name="metadata">Optional metadata to be assigned to the goal structure.</param>
        public static PrimitiveGoalStructure<TBeliefSet> Lift<TBeliefSet>(this IGoal<TBeliefSet> goal, IMetadata metadata)
            where TBeliefSet : IBeliefSet => new(metadata, goal);

        /// <inheritdoc cref="Lift{TBeliefSet}(Desire.Goals.IGoal{TBeliefSet},IMetadata)" />
        public static PrimitiveGoalStructure<TBeliefSet> Lift<TBeliefSet>(this IGoal<TBeliefSet> goal)
            where TBeliefSet : IBeliefSet => goal.Lift(new Metadata());

        /// <summary>
        /// Wraps a goal structure into a desire set.
        /// </summary>
        /// <returns> A desire set. </returns>
        /// <param name="goalStructure">
        /// The goal structure which on its own can function as a desire set. Meaning, the desire set consists of just
        /// a single goal structure.
        /// </param>
        /// <param name="metadata">Optional metadata to be assigned to the desire set.</param>
        public static DesireSet<TBeliefSet> Lift<TBeliefSet>(this IGoalStructure<TBeliefSet> goalStructure, IMetadata metadata)
            where TBeliefSet : IBeliefSet => new(metadata, goalStructure);

        /// <inheritdoc cref="Lift{TBeliefSet}(Desire.GoalStructures.IGoalStructure{TBeliefSet},IMetadata)" />
        public static DesireSet<TBeliefSet> Lift<TBeliefSet>(this IGoalStructure<TBeliefSet> goalStructure)
            where TBeliefSet : IBeliefSet => goalStructure.Lift(new Metadata());
    }
}
