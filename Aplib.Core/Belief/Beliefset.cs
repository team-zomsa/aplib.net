using System.Linq;

namespace Aplib.Core.Belief
{
    /// <summary>
    /// The <see cref="Beliefset"/> class can be inherited to define a set of beliefs for an agent.
    /// All <i>public fields</i> of type <see cref="IBelief"/> that are defined in the inheriting class
    /// are automatically updated when calling <see cref="UpdateBeliefs"/>.
    /// </summary>
    public abstract class Beliefset
    {
        private readonly IBelief[] _beliefs;

        /// <summary>
        /// Initializes a new instance of the <see cref="Beliefset"/> class.
        /// All <i>public fields</i> of type <see cref="IBelief"/> that are defined in the inheriting class
        /// are automatically updated when calling <see cref="UpdateBeliefs"/>.
        /// </summary>
        public Beliefset()
        {
            _beliefs =
                GetType().GetFields()
                .Where(field => field.GetType().IsAssignableFrom(typeof(IBelief)))
                .Select(field => (IBelief)field.GetValue(this))
                .ToArray();
        }

        /// <summary>
        /// Updates all objects of type <see cref="IBelief"/> that are defined as <i>public fields</i> in <see cref="Beliefset"/>.
        /// </summary>
        public void UpdateBeliefs()
        {
            foreach (IBelief belief in _beliefs) belief.UpdateBelief();
        }
    }
}
