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
        /// <inheritdoc cref="FirstOfGoalStructure{TBeliefSet}(Metadata,IGoalStructure{TBeliefSet}[])" />
        public static FirstOfGoalStructure<TBeliefSet> FirstOf<TBeliefSet>
            (Metadata metadata, params IGoalStructure<TBeliefSet>[] children)
            where TBeliefSet : IBeliefSet =>
            new(metadata, children);

        /// <inheritdoc cref="FirstOfGoalStructure{TBeliefSet}(IGoalStructure{TBeliefSet}[])" />
        public static FirstOfGoalStructure<TBeliefSet> FirstOf<TBeliefSet>(params IGoalStructure<TBeliefSet>[] children)
            where TBeliefSet : IBeliefSet =>
            new(children);

        /// <inheritdoc cref="PrimitiveGoalStructure{TBeliefSet}(Metadata,IGoal{TBeliefSet})" />
        public static PrimitiveGoalStructure<TBeliefSet> Primitive<TBeliefSet>
            (Metadata metadata, IGoal<TBeliefSet> goal)
            where TBeliefSet : IBeliefSet =>
            new(metadata, goal);

        /// <inheritdoc cref="PrimitiveGoalStructure{TBeliefSet}(IGoal{TBeliefSet})" />
        public static PrimitiveGoalStructure<TBeliefSet> Primitive<TBeliefSet>(IGoal<TBeliefSet> goal)
            where TBeliefSet : IBeliefSet =>
            new(goal);

        /// <inheritdoc cref="RepeatGoalStructure{TBeliefSet}(Metadata,IGoalStructure{TBeliefSet})" />
        public static RepeatGoalStructure<TBeliefSet> Repeat<TBeliefSet>
            (Metadata metadata, IGoalStructure<TBeliefSet> goalStructure)
            where TBeliefSet : IBeliefSet =>
            new(metadata, goalStructure);

        /// <inheritdoc cref="RepeatGoalStructure{TBeliefSet}(IGoalStructure{TBeliefSet})" />
        public static RepeatGoalStructure<TBeliefSet> Repeat<TBeliefSet>(IGoalStructure<TBeliefSet> goalStructure)
            where TBeliefSet : IBeliefSet =>
            new(goalStructure);

        /// <inheritdoc cref="SequentialGoalStructure{TBeliefSet}(Metadata,IGoalStructure{TBeliefSet}[])" />
        public static SequentialGoalStructure<TBeliefSet> Seq<TBeliefSet>
            (Metadata metadata, params IGoalStructure<TBeliefSet>[] children)
            where TBeliefSet : IBeliefSet =>
            new(metadata, children);

        /// <inheritdoc cref="SequentialGoalStructure{TBeliefSet}(IGoalStructure{TBeliefSet}[])" />
        public static SequentialGoalStructure<TBeliefSet> Seq<TBeliefSet>(params IGoalStructure<TBeliefSet>[] children)
            where TBeliefSet : IBeliefSet =>
            new(children);

        /// <inheritdoc cref="AnyOfTactic{TBeliefSet}(Metadata,System.Func{TBeliefSet,bool},ITactic{TBeliefSet}[])" />
        public static AnyOfTactic<TBeliefSet> AnyOf<TBeliefSet>
            (Metadata metadata, System.Func<TBeliefSet, bool> guard, params ITactic<TBeliefSet>[] subTactics)
            where TBeliefSet : IBeliefSet =>
            new(metadata, guard, subTactics);

        /// <inheritdoc cref="AnyOfTactic{TBeliefSet}(System.Func{TBeliefSet,bool},ITactic{TBeliefSet}[])" />
        public static AnyOfTactic<TBeliefSet> AnyOf<TBeliefSet>
            (System.Func<TBeliefSet, bool> guard, params ITactic<TBeliefSet>[] subTactics)
            where TBeliefSet : IBeliefSet =>
            new(guard, subTactics);

        /// <inheritdoc cref="AnyOfTactic{TBeliefSet}(Metadata,ITactic{TBeliefSet}[])" />
        public static AnyOfTactic<TBeliefSet> AnyOf<TBeliefSet>
            (Metadata metadata, params ITactic<TBeliefSet>[] subTactics)
            where TBeliefSet : IBeliefSet =>
            new(metadata, subTactics);

        /// <inheritdoc cref="AnyOfTactic{TBeliefSet}(ITactic{TBeliefSet}[])" />
        public static AnyOfTactic<TBeliefSet> AnyOf<TBeliefSet>(params ITactic<TBeliefSet>[] subTactics)
            where TBeliefSet : IBeliefSet =>
            new(subTactics);

        /// <inheritdoc cref="FirstOfTactic{TBeliefSet}(Metadata,System.Func{TBeliefSet,bool},ITactic{TBeliefSet}[])" />
        public static FirstOfTactic<TBeliefSet> FirstOf<TBeliefSet>
            (Metadata metadata, System.Func<TBeliefSet, bool> guard, params ITactic<TBeliefSet>[] subTactics)
            where TBeliefSet : IBeliefSet =>
            new(metadata, guard, subTactics);

        /// <inheritdoc cref="FirstOfTactic{TBeliefSet}(System.Func{TBeliefSet,bool},ITactic{TBeliefSet}[])" />
        public static FirstOfTactic<TBeliefSet> FirstOf<TBeliefSet>
            (System.Func<TBeliefSet, bool> guard, params ITactic<TBeliefSet>[] subTactics)
            where TBeliefSet : IBeliefSet =>
            new(guard, subTactics);

        /// <inheritdoc cref="FirstOfTactic{TBeliefSet}(Metadata,ITactic{TBeliefSet}[])"/>
        public static FirstOfTactic<TBeliefSet> FirstOf<TBeliefSet>
            (Metadata metadata, params ITactic<TBeliefSet>[] subTactics)
            where TBeliefSet : IBeliefSet =>
            new(metadata, subTactics);

        /// <inheritdoc cref="FirstOfTactic{TBeliefSet}(ITactic{TBeliefSet}[])"/>
        public static FirstOfTactic<TBeliefSet> FirstOf<TBeliefSet>(params ITactic<TBeliefSet>[] subTactics)
            where TBeliefSet : IBeliefSet =>
            new(subTactics);

        /// <inheritdoc cref="PrimitiveTactic{TBeliefSet}(Metadata,IAction{TBeliefSet},System.Func{TBeliefSet,bool})"/>
        public static PrimitiveTactic<TBeliefSet> Primitive<TBeliefSet>
            (Metadata metadata, IAction<TBeliefSet> action, System.Func<TBeliefSet, bool> guard)
            where TBeliefSet : IBeliefSet =>
            new(metadata, action, guard);

        /// <inheritdoc cref="PrimitiveTactic{TBeliefSet}(IAction{TBeliefSet},System.Func{TBeliefSet,bool})"/>
        public static PrimitiveTactic<TBeliefSet> Primitive<TBeliefSet>
            (IAction<TBeliefSet> action, System.Func<TBeliefSet, bool> guard)
            where TBeliefSet : IBeliefSet =>
            new(action, guard);

        /// <inheritdoc cref="PrimitiveTactic{TBeliefSet}(Metadata,IAction{TBeliefSet})"/>
        public static PrimitiveTactic<TBeliefSet> Primitive<TBeliefSet>(Metadata metadata, IAction<TBeliefSet> action)
            where TBeliefSet : IBeliefSet =>
            new(metadata, action);

        /// <inheritdoc cref="PrimitiveTactic{TBeliefSet}(IAction{TBeliefSet})"/>
        public static PrimitiveTactic<TBeliefSet> Primitive<TBeliefSet>(IAction<TBeliefSet> action)
            where TBeliefSet : IBeliefSet =>
            new(action);

        /// <inheritdoc
        ///     cref="PrimitiveTactic{TBeliefSet}(Metadata,IQueryable{TBeliefSet},System.Func{TBeliefSet,bool})"/>
        public static PrimitiveTactic<TBeliefSet> Primitive<TBeliefSet>
            (Metadata metadata, IQueryable<TBeliefSet> query, System.Func<TBeliefSet, bool> guard)
            where TBeliefSet : IBeliefSet =>
            new(metadata, query, guard);

        /// <inheritdoc cref="PrimitiveTactic{TBeliefSet}(IQueryable{TBeliefSet},System.Func{TBeliefSet,bool})"/>
        public static PrimitiveTactic<TBeliefSet> Primitive<TBeliefSet>
            (IQueryable<TBeliefSet> query, System.Func<TBeliefSet, bool> guard)
            where TBeliefSet : IBeliefSet =>
            new(query, guard);

        /// <inheritdoc cref="PrimitiveTactic{TBeliefSet}(Metadata,IQueryable{TBeliefSet})"/>
        public static PrimitiveTactic<TBeliefSet> Primitive<TBeliefSet>(Metadata metadata, IQueryable<TBeliefSet> query)
            where TBeliefSet : IBeliefSet =>
            new(metadata, query);

        /// <inheritdoc cref="PrimitiveTactic{TBeliefSet}(IQueryable{TBeliefSet})"/>
        public static PrimitiveTactic<TBeliefSet> Primitive<TBeliefSet>(IQueryable<TBeliefSet> query)
            where TBeliefSet : IBeliefSet =>
            new(query);
    }
}
