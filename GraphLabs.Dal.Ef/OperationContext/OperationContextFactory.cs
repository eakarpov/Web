using GraphLabs.Dal.Ef.Infrastructure;
using GraphLabs.DomainModel.Contexts;
using GraphLabs.Site.Core;
using GraphLabs.Site.Core.OperationContext;

namespace GraphLabs.Dal.Ef.OperationContext
{
    /// <summary> ������� <see cref="OperationContextImpl"/> </summary>
    class OperationContextFactory : IOperationContextFactory<IGraphLabsContext>
    {
        /// <summary> ������� �������� </summary>
        public IOperationContext<IGraphLabsContext> Create()
        {
            var ctx = new GraphLabsContext();
            return new OperationContextImpl(new GraphLabsContextImpl(ctx), new ChangesTracker(ctx));
        }
    }
}