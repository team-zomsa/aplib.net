using System;

namespace Aplib.Core
{
    /// <summary>
    /// Describes an action that can be executed and guarded.
    /// </summary>
    /// <typeparam name="TQuery">The type of the query of the action</typeparam>
    public class Action<TQuery>
    {
        private TQuery? _storedResult;

        /// <summary>
        /// Gets or sets the effect of the action.
        /// </summary>
        public System.Action<TQuery> Effect { private get; set; }

        /// <summary>
        /// Gets a value indicating whether the action is actionable.
        /// </summary>
        public bool Actionable => Query!.Invoke() is not (false or null);

        /// <summary>
        /// Gets or sets the query of the action. Can return a value, which is stored and can be used in the effect.
        /// </summary>
        /// <remarks>A boolean value can also be used. If an object is used, use null to mark the action as unactionable.</remarks>
        public Func<TQuery> Query { private get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Action{TQuery}"/> class.
        /// </summary>
        /// <param name="effect">The effect of the action.</param>
        /// <param name="query">THe query of the action.</param>
        public Action(System.Action<TQuery> effect, Func<TQuery> query)
        {
            Effect = effect;
            Query = query;
        }

        /// <summary>
        /// Execute the action against the world.
        /// </summary>
        public void Execute() => Effect.Invoke(_storedResult!);

        /// <summary>
        /// Guard the action against unwanted execution. The result is stored and can be used in the effect.
        /// </summary>
        public void Guard() => _storedResult = Query.Invoke();
    }
}
