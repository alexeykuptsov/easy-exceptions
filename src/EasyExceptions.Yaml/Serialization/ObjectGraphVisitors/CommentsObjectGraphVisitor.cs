using EasyExceptions.Yaml.Core;

namespace EasyExceptions.Yaml.Serialization.ObjectGraphVisitors
{
    public sealed class CommentsObjectGraphVisitor : ChainedObjectGraphVisitor
    {
        public CommentsObjectGraphVisitor(IObjectGraphVisitor<IEmitter> nextVisitor)
            : base(nextVisitor)
        {
        }
    }
}
