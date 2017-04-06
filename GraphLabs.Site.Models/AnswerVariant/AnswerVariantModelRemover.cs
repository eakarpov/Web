using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Contexts;
using GraphLabs.DomainModel.Extensions;
using GraphLabs.Site.Core.OperationContext;
using GraphLabs.Site.Models.Infrastructure;

namespace GraphLabs.Site.Models.AnswerVariant
{
    internal sealed class AnswerVariantModelRemover : AbstractModelRemover<AnswerVariantModel, DomainModel.AnswerVariant>
    {
        public AnswerVariantModelRemover(IOperationContextFactory<IGraphLabsContext> operationContextFactory)
            : base(operationContextFactory)
        {
        }

        /// <summary> Существует ли соответствующая запись в БД? </summary>
        /// <remarks> При реализации - просто проверить ключ, в базу лазить НЕ НАДО </remarks>
        protected override bool ExistsInDatabase(long id)
        {
            return id != 0;
        }
    }
}
