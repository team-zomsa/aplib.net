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
        /// <returns>
        /// A primitive tactic, whose guard always returns true.
        /// If the action implements <see cref="IDocumented"/>, it's name will be prefixed with an indicator that it
        /// has been lifted.
        /// </returns>
        /// <param name="action">
        /// The action which on its own can function as a tactic. Meaning, the tactic consists of just a single action.
        /// </param>
        public static PrimitiveTactic<TBeliefSet> Lift<TBeliefSet>(this IAction<TBeliefSet> action)
            where TBeliefSet : IBeliefSet
        {
            string mostDerivedActionType = action.GetType().Name;

            IMetadata metadata = action is not IDocumented actionWithMetadata
                ? new Metadata($"Lifted {mostDerivedActionType}")
                : new Metadata(!string.IsNullOrWhiteSpace(actionWithMetadata.Metadata.Name)
                    ? $"[Lifted {mostDerivedActionType}] {actionWithMetadata.Metadata.Name}"
                    : $"Lifted {mostDerivedActionType}",
                    actionWithMetadata.Metadata.Description);

            return new PrimitiveTactic<TBeliefSet>(metadata, action: action);
        }

        /// <summary>
        /// Wraps a queryable action into a tactic.
        /// </summary>
        /// <returns>
        /// A primitive tactic, whose guard always returns true.
        /// If the action implements <see cref="IDocumented"/>, it's name will be prefixed with an indicator that it
        /// has been lifted.
        /// </returns>
        /// <param name="action">
        /// The action which on its own can function as a tactic. Meaning, the tactic consists of just a single action.
        /// </param>
        public static PrimitiveTactic<TBeliefSet> Lift<TBeliefSet>(this IQueryable<TBeliefSet> action)
            where TBeliefSet : IBeliefSet
        {
            string mostDerivedActionType = action.GetType().Name;

            IMetadata metadata = action is not IDocumented actionWithMetadata
                ? new Metadata($"Lifted {mostDerivedActionType}")
                : new Metadata(!string.IsNullOrWhiteSpace(actionWithMetadata.Metadata.Name)
                    ? $"[Lifted {mostDerivedActionType}] {actionWithMetadata.Metadata.Name}"
                    : $"Lifted {mostDerivedActionType}",
                    actionWithMetadata.Metadata.Description);

            return new PrimitiveTactic<TBeliefSet>(metadata, queryAction: action);
        }

        /// <summary>
        /// Wraps a goal into a goal structure.
        /// </summary>
        /// <returns>
        /// A primitive goal structure.
        /// If the goal implements <see cref="IDocumented"/>, it's name will be prefixed with an indicator that it
        /// has been lifted.
        /// </returns>
        /// <param name="goal">
        /// The goal which on its own can function as a goal structure. Meaning, the goal structure consists of just a
        /// single goal.
        /// </param>
        public static PrimitiveGoalStructure<TBeliefSet> Lift<TBeliefSet>(this IGoal<TBeliefSet> goal)
            where TBeliefSet : IBeliefSet
        {
            string mostDerivedGoalType = goal.GetType().Name;

            IMetadata metadata = goal is not IDocumented goalWithMetadata
                ? new Metadata($"Lifted {mostDerivedGoalType}")
                : new Metadata(!string.IsNullOrWhiteSpace(goalWithMetadata.Metadata.Name)
                    ? $"[Lifted {mostDerivedGoalType}] {goalWithMetadata.Metadata.Name}"
                    : $"Lifted {mostDerivedGoalType}",
                    goalWithMetadata.Metadata.Description);

            return new PrimitiveGoalStructure<TBeliefSet>(metadata, goal);
        }

        /// <summary>
        /// Wraps a goal structure into a desire set.
        /// </summary>
        /// <returns>
        /// A desire set.
        /// If the goal structure implements <see cref="IDocumented"/>, it's name will be prefixed with an indicator
        /// that it has been lifted.
        /// </returns>
        /// <param name="goalStructure">
        /// The goal structure which on its own can function as a desire set. Meaning, the desire set consists of just
        /// a single goal structure.
        /// </param>
        public static DesireSet<TBeliefSet> Lift<TBeliefSet>(this IGoalStructure<TBeliefSet> goalStructure)
            where TBeliefSet : IBeliefSet
        {
            string mostDerivedGoalStructureType = goalStructure.GetType().Name;

            IMetadata metadata = goalStructure is not IDocumented goalStructureWithMetadata
                ? new Metadata($"Lifted {mostDerivedGoalStructureType}")
                : new Metadata(!string.IsNullOrWhiteSpace(goalStructureWithMetadata.Metadata.Name)
                    ? $"[Lifted {mostDerivedGoalStructureType}] {goalStructureWithMetadata.Metadata.Name}"
                    : $"Lifted {mostDerivedGoalStructureType}",
                    goalStructureWithMetadata.Metadata.Description);

            return new DesireSet<TBeliefSet>(metadata, goalStructure);
        }
    }
}
