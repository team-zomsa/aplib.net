using Aplib.Core.Belief.Beliefs;
using System.Linq;

namespace Aplib.Core.Belief.BeliefSets
{
    /// <summary>
    /// The <see cref="BeliefSet"/> class can be inherited to define a set of beliefs for an agent.
    /// All <i>public fields</i> of type <see cref="IBelief"/> that are defined in the inheriting class
    /// are automatically updated when calling <see cref="UpdateBeliefs"/>.
    /// </summary>
    public abstract class BeliefSet : IBeliefSet
    {
        /// <summary>
        /// An array storing all <i>public fields</i> of type <see cref="IBelief"/> that are defined in the inheriting class.
        /// </summary>
        private readonly IBelief[] _beliefs;

        /// <summary>
        /// Initializes a new instance of the <see cref="BeliefSet"/> class,
        /// and stores all <i>public fields</i> of type <see cref="IBelief"/> (that have been defined in the inheriting class) in an array.
        /// All public <see cref="IBelief"/> fields are then automatically updated when calling <see cref="UpdateBeliefs"/>.
        /// </summary>
        protected BeliefSet()
        {
            _beliefs = GetType()
                .GetFields()
                .Where(field => typeof(IBelief).IsAssignableFrom(field.FieldType))
                .Select(field => (IBelief)field.GetValue(this))
                .ToArray();
        }

        /// <summary>
        /// Updates all objects of type <see cref="IBelief"/> that are defined as <i>public fields</i> in the inheriting class.
        /// </summary>
        public void UpdateBeliefs()
        {
            foreach (IBelief belief in _beliefs) belief.UpdateBelief();
        }
    }
}
