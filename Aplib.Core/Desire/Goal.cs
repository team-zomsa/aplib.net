namespace Aplib.Core.Desire
{
    /// <summary>
    /// A goal effectively combines a heuristicFunction with a tactic, and aims to meet the heuristicFunction by
    /// applying the tactic. Goals are combined in a <see cref="GoalStructure"/>, and are used to prepare tests or do
    /// the testing.
    /// </summary>
    /// <seealso cref="GoalStructure"/>
    public class Goal
    {
        /// <summary>
        /// The goal is considered to be completed, when the distance of the <see cref="CurrentHeuristics"/> is below
        /// this value.
        /// </summary>
        protected double epsilon;

        /// <summary>
        /// The abstract definition of what is means to test the Goal's heuristicFunction. Returns <see cref="Heuristics"/>, as
        /// they represent how close we are to matching the heuristicFunction, and if the goal is completed.
        /// </summary>
        /// <seealso cref="Goal.Evaluate"/>
        /// <remarks>The paper mentions predicates, yet the Java Aplib uses heuristics. We use heuristics as well.</remarks>
        public delegate Heuristics HeuristicFunction();


        /// <summary>
        /// Gets the <see cref="Heuristics"/> of the current state of the game.
        /// </summary>
        /// <remarks>If no heuristics have been calculated yet, they will be calculated first.</remarks>
        public virtual Heuristics CurrentHeuristics => _currentHeuristics ??= heuristicFunction.Invoke();

        /// <summary>
        /// The name used to display the current goal during debugging, logging, or general overviews.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The description used to describe the current goal during debugging, logging, or general overviews.
        /// </summary>
        public string Description { get; }


        /// <summary>
        /// The concrete implementation of this Goal's <see cref="HeuristicFunction"/>. Used to test whether this goal is
        /// completed.
        /// </summary>
        /// <seealso cref="Evaluate"/>
        protected HeuristicFunction heuristicFunction;

        /// <summary>
        /// The backing field of <see cref="Heuristics"/>.
        /// </summary>
        private Heuristics? _currentHeuristics;

        /// <summary>
        /// The <see cref="Tactic"/> used to achieve this <see cref="Goal"/>, which is executed during every iteration
        /// of the BDI cycle.
        /// </summary>
        /// <seealso cref="Iterate()"/>
        private readonly Tactic _tactic;

        /// <summary>
        /// Creates a new goal with specified arguments. Upon creation, the <see cref="CurrentHeuristics"/> will be
        /// set to <see cref="Heuristics.Default"/>.
        /// </summary>
        /// <param name="tactic">The tactic used to approach this goal.</param>
        /// <param name="heuristicFunction">The heuristicFunction which defines whether a goal is reached</param>
        /// <param name="name">The name of this goal, used to quickly display this goal in several contexts.</param>
        /// <param name="description">The description of this goal, used to explain this goal in several contexts.</param>
        /// <param name="epsilon">
        /// The goal is considered to be completed, when the distance of the <see cref="CurrentHeuristics"/> is below
        /// this value.
        /// </param>
        public Goal(Tactic tactic, HeuristicFunction heuristicFunction, string name, string description, double epsilon = 0.005d)
        {
            _tactic = tactic;
            this.heuristicFunction = heuristicFunction;
            Name = name;
            Description = description;
            this.epsilon = epsilon;
        }


        /// <summary>
        /// Performs the next steps needed to be taken to approach this goal. Effectively this means that one BDI
        /// cycle will be executed.
        /// </summary>
        public void Iterate()
        {
            _tactic.IterateBdiCycle();
        }

        /// <summary>
        /// Tests whether the goal has been achieved, bases on the <see cref="heuristicFunction"/> and the
        /// <see cref="CurrentHeuristics"/>. When the distance of the heuristics is smaller than <see cref="epsilon"/>,
        /// the goal is considered to be completed.
        /// </summary>
        /// <returns>A boolean representing whether the goal is considered to be completed.</returns>
        /// <seealso cref="epsilon"/>
        public bool Evaluate()
        {
            return CurrentHeuristics.Distance < epsilon;
        }
    }
}
