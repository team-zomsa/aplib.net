using System.Collections.Generic;

namespace Aplib.Core
{
    /// <summary>
    /// An interface that allows defining the structure loggable objects through a tree.
    /// </summary>
    public interface ILoggable : IDocumented
    {
        /// <summary>
        /// Gets the children of the loggable object.
        /// </summary>
        /// <returns>The children of the loggable object.</returns>
        public IEnumerable<ILoggable> GetChildren();

        /// <summary>
        /// Generates a log tree of the loggable object.
        /// </summary>
        /// <param name="depth">The depth of the first node of log tree.</param>
        /// <returns>The first node of the log tree.</returns>
        public LogNode GetLogTree(int depth = 0)
        {
            LogNode root = new(this, depth);
            foreach (ILoggable child in GetChildren())
            {
                root.Children.Add(child.GetLogTree(depth + 1));
            }
            return root;
        }
    }

    /// <summary>
    /// Represents a node in the log tree.
    /// </summary>
    public class LogNode
    {
        /// <summary>
        /// The loggable object of the node.
        /// </summary>
        public ILoggable Loggable { get; }

        /// <summary>
        /// The depth at which this node resides.
        /// </summary>
        /// <value></value>
        public int Depth { get; }

        /// <summary>
        /// The children of the node.
        /// </summary>
        /// <value></value>
        public List<LogNode> Children { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogNode"/> class.
        /// </summary>
        /// <param name="loggable"> The loggable object of the node.</param>
        /// <param name="depth"> The depth at which this node resides.</param>
        public LogNode(ILoggable loggable, int depth)
        {
            Loggable = loggable;
            Depth = depth;
            Children = new List<LogNode>();
        }
    }
}