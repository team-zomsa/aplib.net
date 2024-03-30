using Aplib.Core.Belief;
using Aplib.Core.Desire.Goals;
using System.Collections.Generic;

namespace Aplib.Core.Desire
{
    /// <summary>
    /// Describes a structure of goals that need to be fulfilled.
    /// </summary>
    /// <remarks>
    ///
    /// </remarks>
    public abstract class GoalStructure<TBeliefSet>
        where TBeliefSet : IBeliefSet
    {
        /// <summary>
        /// The children of the goal structure.
        /// </summary>
        protected IList<GoalStructure<TBeliefSet>> _children;

        /// <summary>
        /// The goalstructure that is currently being fulfilled.
        /// </summary>
        protected GoalStructure<TBeliefSet>? _currentGoalStructure;

        /// <summary>
        /// Gets or sets the state of the goal structure.
        /// </summary>
        /// <remarks>
        /// By default, the state is set to <see cref="GoalStructureState.Unfinished"/>.
        /// However, this can be changed by the goal structure itself.
        /// </remarks>
        public GoalStructureState State { get; protected set; } = GoalStructureState.Unfinished; // TODO

        /// <summary>
        /// Initializes a new instance of the <see cref="GoalStructure"/> class.
        /// </summary>
        /// <param name="children">The children of the goal structure.</param>
        protected GoalStructure(IList<GoalStructure<TBeliefSet>> children)
        {
            _children = children;
        }

        /// <summary>
        /// Interrupts the current goal structure.
        /// </summary>
        public void Interrupt()
        {
            foreach (GoalStructure<TBeliefSet> child in _children)
                child.Interrupt();

            ProcessInterrupt();
        }

        /// <summary>
        /// Gets the current goal using the given <see cref="IBeliefSet"/>.
        /// </summary>
        /// <param name="beliefSet">The belief set of the agent.</param>
        /// <returns>The current goal to be fulfilled.</returns>
        public abstract Goal? GetCurrentGoal(TBeliefSet beliefSet);

        /// <summary>
        /// Processes the interrupt thrown by the <see cref="DesireSet"/>.
        /// </summary>
        /// <remarks>
        ///<para>
        /// A goal structure may want to do something when it is interrupted.
        /// This method is always called when another goal structure interrupts this one.
        ///</para>
        /// <para>
        /// The default implementation does nothing.
        /// </para>
        /// </remarks>
        protected virtual void ProcessInterrupt() { }
    }
}
