using System.Linq;

namespace Aplib.Core.Belief
{
    public class Beliefset
    {
        private readonly IBelief[] _beliefs;

        public Beliefset()
        {
            _beliefs =
                GetType().GetFields()
                .Where(field => field.GetType().IsAssignableFrom(typeof(IBelief)))
                .Select(field => (IBelief)field.GetValue(this))
                .ToArray();
        }

        public void UpdateBeliefs()
        {
            foreach (IBelief belief in _beliefs) belief.UpdateBelief();
        }
    }
}
