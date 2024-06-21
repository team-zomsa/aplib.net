using System.Collections.Generic;

namespace Aplib.Core.Logging
{
    /// <summary>
    /// An interface that allows defining the structure of loggable objects through a <see cref="LogNode"/> tree.
    /// The only method that needs to be implemented in loggable classes is the <see cref="GetLogChildren" /> method;
    /// the structure of the log tree is generated automatically when calling the <see cref="GetLogTree" /> method.
    /// </summary>
    public interface ILoggable : IDocumented
    {
        /// <summary>
        /// Gets the children of the loggable object.
        /// </summary>
        /// <returns>The children of the loggable object.</returns>
        public IEnumerable<ILoggable> GetLogChildren();

        /// <summary>
        /// Generates a log tree of the loggable object.
        /// </summary>
        /// <param name="depth">The depth of this node in the log tree.</param>
        /// <returns>The root node of the log tree.</returns>
        public LogNode GetLogTree(int depth = 0)
        {
            LogNode root = new(this, depth);

            foreach (ILoggable child in GetLogChildren())
                root.Children.Add(child.GetLogTree(depth + 1));

            return root;
        }
    }
}
