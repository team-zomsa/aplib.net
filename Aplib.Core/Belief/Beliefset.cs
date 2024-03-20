using System.Linq;

namespace Aplib.Core.Belief
{
    /// <summary>
    /// The <see cref="BeliefSet"/> class can be inherited to define a set of beliefs for an agent.
    /// </summary>
    /// <remarks>
    /// All <i>public fields</i> of type <see cref="IBelief"/> that are defined in the inheriting class
    /// are automatically updated when calling <see cref="UpdateBeliefs"/>.
    /// </remarks>
    public abstract class BeliefSet
    {
        private readonly IBelief[] _beliefs;

        /// <summary>
        /// Initializes a new instance of the <see cref="BeliefSet"/> class.
        /// </summary>
        /// <remarks>
        /// All <i>public fields</i> of type <see cref="IBelief"/> that are defined in the inheriting class
        /// are automatically updated when calling <see cref="UpdateBeliefs"/>.
        /// </remarks>
        protected BeliefSet()
        {
            _beliefs =
                GetType().GetFields()
                .Where(field => typeof(IBelief).IsAssignableFrom(field.FieldType))
                .Select(field => (IBelief)field.GetValue(this))
                .ToArray();
        }

        /// <summary>
        /// Updates all objects of type <see cref="IBelief"/> that are defined as <i>public fields</i> in <see cref="BeliefSet"/>.
        /// </summary>
        public void UpdateBeliefs()
        {
            foreach (IBelief belief in _beliefs) belief.UpdateBelief();
        }
    }
}
