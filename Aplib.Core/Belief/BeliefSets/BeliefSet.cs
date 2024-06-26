// This program has been developed by students from the bachelor Computer Science at Utrecht
// University within the Software Project course.
// Copyright Utrecht University (Department of Information and Computing Sciences)

using Aplib.Core.Belief.Beliefs;
using System.Linq;

namespace Aplib.Core.Belief.BeliefSets
{
    /// <summary>
    /// This class can be inherited to define a set of beliefs for an agent.
    /// All <i>public fields</i> defined in the inheriting class that implement <see cref="IBelief" /> 
    /// are automatically updated when calling <see cref="UpdateBeliefs" />.
    /// </summary>
    public abstract class BeliefSet : IBeliefSet
    {
        /// <summary>
        /// An array storing all <i>public fields</i> of type <see cref="IBelief" /> that are defined in the inheriting
        /// class.
        /// </summary>
        private readonly IBelief[] _beliefs;

        /// <summary>
        /// Initializes a new instance of the <see cref="BeliefSet"/> class and stores all <i>public fields</i> defined
        /// in the inheriting class that implement <see cref="IBelief" /> in an array.
        /// All public <see cref="IBelief" /> fields are then automatically updated when calling
        /// <see cref="UpdateBeliefs" />.
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
        /// Updates all objects that implement <see cref="IBelief"/> and that are defined as <i>public fields</i> in the
        /// inheriting class.
        /// </summary>
        public void UpdateBeliefs()
        {
            foreach (IBelief belief in _beliefs) belief.UpdateBelief();
        }
    }
}
