﻿namespace Aplib.Extensions
{
    /// <summary>
    /// Defines a pathfinding algorithm used to find a path from a starting point to an end point.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the path.</typeparam>
    public interface IPathfinder<T>
    {
        /// <summary>
        /// Finds a path from the specified starting point to the specified end point.
        /// </summary>
        /// <param name="begin">The starting point of the path.</param>
        /// <param name="end">The end point of the path.</param>
        /// <returns>
        /// A read-only span of elements representing the path from the starting point to the end point.
        /// </returns>
        public System.ReadOnlySpan<T> FindPath(T begin, T end);

        /// <summary>
        /// Gets the next step towards the specified end point from the current point in the path.
        /// </summary>
        /// <param name="current">The current point in the path.</param>
        /// <param name="end">The end point of the path.</param>
        /// <returns>The next step towards the end point.</returns>
        public T GetNextStep(T current, T end);

        /// <summary>
        /// Tries to get the next step towards the specified end point from the current point in the path.
        /// </summary>
        /// <param name="current">The current point in the path.</param>
        /// <param name="end">The end point of the path.</param>
        /// <param name="nextStep">
        /// When this method returns, this contains the next step towards the end point if such a next step exists,
        /// or the default value of type <typeparamref name="T"/> if there is no next step.
        /// </param>
        /// <returns>
        /// <c>true</c> if the next step towards the end point is obtained successfully. Otherwise, <c>false</c>.
        /// </returns>
        public bool TryGetNextStep(T current, T end, out T nextStep);
    }
}
