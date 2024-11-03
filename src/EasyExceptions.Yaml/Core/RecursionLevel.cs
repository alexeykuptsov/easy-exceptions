using System;

namespace EasyExceptions.Yaml.Core
{
    /// <summary>
    /// Keeps track of the <see cref="current"/> recursion level,
    /// and throws <see cref="MaximumRecursionLevelReachedException"/>
    /// whenever <see cref="Maximum"/> is reached.
    /// </summary>
    internal sealed class RecursionLevel
    {
        private int current;
        public int Maximum { get; }

        public RecursionLevel(int maximum)
        {
            Maximum = maximum;
        }

        /// <summary>
        /// Increments the <see cref="current"/> recursion level,
        /// and throws <see cref="MaximumRecursionLevelReachedException"/>
        /// if <see cref="Maximum"/> is reached.
        /// </summary>
        public void Increment()
        {
            if (!TryIncrement())
            {
                throw new MaximumRecursionLevelReachedException("Maximum level of recursion reached");
            }
        }

        /// <summary>
        /// Increments the <see cref="current"/> recursion level,
        /// and returns whether <see cref="current"/> is still less than <see cref="Maximum"/>.
        /// </summary>
        public bool TryIncrement()
        {
            if (current < Maximum)
            {
                ++current;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Decrements the <see cref="current"/> recursion level.
        /// </summary>
        public void Decrement()
        {
            if (current == 0)
            {
                throw new InvalidOperationException("Attempted to decrement RecursionLevel to a negative value");
            }
            --current;
        }
    }
}
