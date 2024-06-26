// This program has been developed by students from the bachelor Computer Science at Utrecht
// University within the Software Project course.
// Copyright Utrecht University (Department of Information and Computing Sciences)

using Aplib.Core.Belief.BeliefSets;
using Aplib.Core.Desire.Goals;
using Aplib.Core.Desire.GoalStructures;
using Aplib.Core.Intent.Actions;
using Aplib.Core.Intent.Tactics;

namespace Aplib.Core
{
    /// <summary>
    /// Convenience class containing static methods for creating goal structures and tactics.
    /// </summary>
    public static class Combinators
    {
        #region GoalStructure combinators

        /// <inheritdoc cref="FirstOfGoalStructure{TBeliefSet}(IMetadata,IGoalStructure{TBeliefSet}[])" />
        public static FirstOfGoalStructure<TBeliefSet> FirstOf<TBeliefSet>
            (IMetadata metadata, params IGoalStructure<TBeliefSet>[] children)
            where TBeliefSet : IBeliefSet =>
            new(metadata, children);

        /// <inheritdoc cref="FirstOfGoalStructure{TBeliefSet}(IGoalStructure{TBeliefSet}[])" />
        public static FirstOfGoalStructure<TBeliefSet> FirstOf<TBeliefSet>(params IGoalStructure<TBeliefSet>[] children)
            where TBeliefSet : IBeliefSet =>
            new(children);

        /// <inheritdoc cref="PrimitiveGoalStructure{TBeliefSet}(IMetadata,IGoal{TBeliefSet})" />
        public static PrimitiveGoalStructure<TBeliefSet> Primitive<TBeliefSet>
            (IMetadata metadata, IGoal<TBeliefSet> goal)
            where TBeliefSet : IBeliefSet =>
            new(metadata, goal);

        /// <inheritdoc cref="PrimitiveGoalStructure{TBeliefSet}(IGoal{TBeliefSet})" />
        public static PrimitiveGoalStructure<TBeliefSet> Primitive<TBeliefSet>(IGoal<TBeliefSet> goal)
            where TBeliefSet : IBeliefSet =>
            new(goal);

        /// <inheritdoc cref="RepeatGoalStructure{TBeliefSet}(IMetadata,IGoalStructure{TBeliefSet})" />
        public static RepeatGoalStructure<TBeliefSet> Repeat<TBeliefSet>
            (IMetadata metadata, IGoalStructure<TBeliefSet> goalStructure)
            where TBeliefSet : IBeliefSet =>
            new(metadata, goalStructure);

        /// <inheritdoc cref="RepeatGoalStructure{TBeliefSet}(IGoalStructure{TBeliefSet})" />
        public static RepeatGoalStructure<TBeliefSet> Repeat<TBeliefSet>(IGoalStructure<TBeliefSet> goalStructure)
            where TBeliefSet : IBeliefSet =>
            new(goalStructure);

        /// <inheritdoc cref="SequentialGoalStructure{TBeliefSet}(IMetadata,IGoalStructure{TBeliefSet}[])" />
        public static SequentialGoalStructure<TBeliefSet> Seq<TBeliefSet>
            (IMetadata metadata, params IGoalStructure<TBeliefSet>[] children)
            where TBeliefSet : IBeliefSet =>
            new(metadata, children);

        /// <inheritdoc cref="SequentialGoalStructure{TBeliefSet}(IGoalStructure{TBeliefSet}[])" />
        public static SequentialGoalStructure<TBeliefSet> Seq<TBeliefSet>(params IGoalStructure<TBeliefSet>[] children)
            where TBeliefSet : IBeliefSet =>
            new(children);

        #endregion

        #region Tactic combinators

        /// <inheritdoc cref="RandomTactic{TBeliefSet}(IMetadata,System.Predicate{TBeliefSet},ITactic{TBeliefSet}[])" />
        public static RandomTactic<TBeliefSet> Random<TBeliefSet>
            (IMetadata metadata, System.Predicate<TBeliefSet> guard, params ITactic<TBeliefSet>[] subtactics)
            where TBeliefSet : IBeliefSet =>
            new(metadata, guard, subtactics);

        /// <inheritdoc cref="RandomTactic{TBeliefSet}(System.Predicate{TBeliefSet},ITactic{TBeliefSet}[])" />
        public static RandomTactic<TBeliefSet> Random<TBeliefSet>
            (System.Predicate<TBeliefSet> guard, params ITactic<TBeliefSet>[] subtactics)
            where TBeliefSet : IBeliefSet =>
            new(guard, subtactics);

        /// <inheritdoc cref="RandomTactic{TBeliefSet}(IMetadata,ITactic{TBeliefSet}[])" />
        public static RandomTactic<TBeliefSet> Random<TBeliefSet>
            (IMetadata metadata, params ITactic<TBeliefSet>[] subtactics)
            where TBeliefSet : IBeliefSet =>
            new(metadata, subtactics);

        /// <inheritdoc cref="RandomTactic{TBeliefSet}(ITactic{TBeliefSet}[])" />
        public static RandomTactic<TBeliefSet> Random<TBeliefSet>(params ITactic<TBeliefSet>[] subtactics)
            where TBeliefSet : IBeliefSet =>
            new(subtactics);

        /// <inheritdoc
        ///     cref="FirstOfTactic{TBeliefSet}(IMetadata,System.Predicate{TBeliefSet},ITactic{TBeliefSet}[])" />
        public static FirstOfTactic<TBeliefSet> FirstOf<TBeliefSet>
            (IMetadata metadata, System.Predicate<TBeliefSet> guard, params ITactic<TBeliefSet>[] subtactics)
            where TBeliefSet : IBeliefSet =>
            new(metadata, guard, subtactics);

        /// <inheritdoc cref="FirstOfTactic{TBeliefSet}(System.Predicate{TBeliefSet},ITactic{TBeliefSet}[])" />
        public static FirstOfTactic<TBeliefSet> FirstOf<TBeliefSet>
            (System.Predicate<TBeliefSet> guard, params ITactic<TBeliefSet>[] subtactics)
            where TBeliefSet : IBeliefSet =>
            new(guard, subtactics);

        /// <inheritdoc cref="FirstOfTactic{TBeliefSet}(IMetadata,ITactic{TBeliefSet}[])"/>
        public static FirstOfTactic<TBeliefSet> FirstOf<TBeliefSet>
            (IMetadata metadata, params ITactic<TBeliefSet>[] subtactics)
            where TBeliefSet : IBeliefSet =>
            new(metadata, subtactics);

        /// <inheritdoc cref="FirstOfTactic{TBeliefSet}(ITactic{TBeliefSet}[])"/>
        public static FirstOfTactic<TBeliefSet> FirstOf<TBeliefSet>(params ITactic<TBeliefSet>[] subtactics)
            where TBeliefSet : IBeliefSet =>
            new(subtactics);

        /// <inheritdoc cref="PrimitiveTactic{TBeliefSet}(IMetadata,IAction{TBeliefSet},System.Predicate{TBeliefSet})"/>
        public static PrimitiveTactic<TBeliefSet> Primitive<TBeliefSet>
            (IMetadata metadata, IAction<TBeliefSet> action, System.Predicate<TBeliefSet> guard)
            where TBeliefSet : IBeliefSet =>
            new(metadata, action, guard);

        /// <inheritdoc cref="PrimitiveTactic{TBeliefSet}(IAction{TBeliefSet},System.Predicate{TBeliefSet})"/>
        public static PrimitiveTactic<TBeliefSet> Primitive<TBeliefSet>
            (IAction<TBeliefSet> action, System.Predicate<TBeliefSet> guard)
            where TBeliefSet : IBeliefSet =>
            new(action, guard);

        /// <inheritdoc cref="PrimitiveTactic{TBeliefSet}(IMetadata,IAction{TBeliefSet})"/>
        public static PrimitiveTactic<TBeliefSet> Primitive<TBeliefSet>(IMetadata metadata, IAction<TBeliefSet> action)
            where TBeliefSet : IBeliefSet =>
            new(metadata, action);

        /// <inheritdoc cref="PrimitiveTactic{TBeliefSet}(IAction{TBeliefSet})"/>
        public static PrimitiveTactic<TBeliefSet> Primitive<TBeliefSet>(IAction<TBeliefSet> action)
            where TBeliefSet : IBeliefSet =>
            new(action);

        /// <inheritdoc
        ///     cref="PrimitiveTactic{TBeliefSet}(IMetadata,IQueryable{TBeliefSet},System.Predicate{TBeliefSet})"/>
        public static PrimitiveTactic<TBeliefSet> Primitive<TBeliefSet>
            (IMetadata metadata, IQueryable<TBeliefSet> queryAction, System.Predicate<TBeliefSet> guard)
            where TBeliefSet : IBeliefSet =>
            new(metadata, queryAction, guard);

        /// <inheritdoc cref="PrimitiveTactic{TBeliefSet}(IQueryable{TBeliefSet},System.Predicate{TBeliefSet})"/>
        public static PrimitiveTactic<TBeliefSet> Primitive<TBeliefSet>
            (IQueryable<TBeliefSet> queryAction, System.Predicate<TBeliefSet> guard)
            where TBeliefSet : IBeliefSet =>
            new(queryAction, guard);

        /// <inheritdoc cref="PrimitiveTactic{TBeliefSet}(IMetadata,IQueryable{TBeliefSet})"/>
        public static PrimitiveTactic<TBeliefSet> Primitive<TBeliefSet>
            (IMetadata metadata, IQueryable<TBeliefSet> queryAction)
            where TBeliefSet : IBeliefSet =>
            new(metadata, queryAction);

        /// <inheritdoc cref="PrimitiveTactic{TBeliefSet}(IQueryable{TBeliefSet})"/>
        public static PrimitiveTactic<TBeliefSet> Primitive<TBeliefSet>(IQueryable<TBeliefSet> queryAction)
            where TBeliefSet : IBeliefSet =>
            new(queryAction);

        #endregion
    }
}
