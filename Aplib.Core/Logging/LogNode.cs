using System.Collections.Generic;

namespace Aplib.Core.Logging
{
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
        public int Depth { get; }

        /// <summary>
        /// The children of the node.
        /// </summary>
        public List<LogNode> Children { get; }

        /// <summary>
        /// Initialize a new <see cref="LogNode" /> from a given loggable object,
        /// the depth, and optionally a list of children.
        /// </summary>
        /// <param name="loggable">The loggable object of the node.</param>
        /// <param name="depth">The depth at which this node resides.</param>
        /// <param name="children">
        /// The children of the node.
        /// It is assumed the children have a correct depth set.
        /// If omitted, an empty list will be used.
        /// </param>
        public LogNode(ILoggable loggable, int depth, List<LogNode> children)
        {
            Loggable = loggable;
            Depth = depth;
            Children = children;
        }

        /// <inheritdoc />
        public LogNode(ILoggable loggable, int depth) : this(loggable, depth, new List<LogNode>()) { }
    }
}
