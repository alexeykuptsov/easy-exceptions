namespace EasyExceptions.Yaml.Serialization
{
    /// <summary>
    /// Defines a strategy that walks through an object graph.
    /// </summary>
    public interface IObjectGraphTraversalStrategy
    {
        /// <summary>
        /// Traverses the specified object graph.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="visitor">An <see cref="IObjectGraphVisitor{TContext}"/> that is to be notified during the traversal.</param>
        /// <param name="context">A <typeparamref name="TContext" /> that will be passed to the <paramref name="visitor" />.</param>
        void Traverse<TContext>(IObjectDescriptor graph, IObjectGraphVisitor<TContext> visitor, TContext context);
    }
}
