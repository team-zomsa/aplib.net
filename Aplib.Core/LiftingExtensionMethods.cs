using Aplib.Core.Belief.BeliefSets;
using Aplib.Core.Desire;
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
        /// Wraps an action into a tactic.
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
            IMetadata metadata = action is not IDocumented actionWithMetadata
                ? new Metadata($"Lifted {nameof(IAction<TBeliefSet>)}")
                : new Metadata(!string.IsNullOrWhiteSpace(actionWithMetadata.Metadata.Name)
                    ? $"[Lifted {nameof(IAction<TBeliefSet>)}] {actionWithMetadata.Metadata.Name}"
                    : $"Lifted {nameof(IAction<TBeliefSet>)}",
                    actionWithMetadata.Metadata.Description);

            return new PrimitiveTactic<TBeliefSet>(action, metadata);
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
            IMetadata metadata = goal is not IDocumented goalWithMetadata
                ? new Metadata($"Lifted {nameof(IGoal<TBeliefSet>)}")
                : new Metadata(!string.IsNullOrWhiteSpace(goalWithMetadata.Metadata.Name)
                    ? $"[Lifted {nameof(IGoal<TBeliefSet>)}] {goalWithMetadata.Metadata.Name}"
                    : $"Lifted {nameof(IGoal<TBeliefSet>)}",
                    goalWithMetadata.Metadata.Description);

            return new PrimitiveGoalStructure<TBeliefSet>(goal, metadata);
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
            IMetadata metadata = goalStructure is not IDocumented goalStructureWithMetadata
                ? new Metadata($"Lifted {nameof(IGoalStructure<TBeliefSet>)}")
                : new Metadata(!string.IsNullOrWhiteSpace(goalStructureWithMetadata.Metadata.Name)
                    ? $"[Lifted {nameof(IGoalStructure<TBeliefSet>)}] {goalStructureWithMetadata.Metadata.Name}"
                    : $"Lifted {nameof(IGoalStructure<TBeliefSet>)}",
                    goalStructureWithMetadata.Metadata.Description);

            return new DesireSet<TBeliefSet>(goalStructure, metadata);
        }
    }
}
