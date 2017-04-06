using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphLabs.Site.Models.Infrastructure;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Contexts;
using GraphLabs.DomainModel.Extensions;
using GraphLabs.Site.Core.OperationContext;
using GraphLabs.Site.Models.TestPoolEntry;
using GraphLabs.Site.Models.TestQuestion;
using Microsoft.Practices.ObjectBuilder2;

namespace GraphLabs.Site.Models.AnswerVariant
{
    internal sealed class AnswerVariantModelSaver : AbstractModelSaver<AnswerVariantModel, DomainModel.AnswerVariant>
    {


        public AnswerVariantModelSaver(
            IOperationContextFactory<IGraphLabsContext> operationContextFactory
            ) : base(operationContextFactory)
        {
        }

        protected override Action<DomainModel.AnswerVariant> GetEntityInitializer(AnswerVariantModel model, IEntityQuery query)
        {
            var entity = query.Get<DomainModel.TestQuestion>(model.TestQuestion);
            return g =>
            {
                g.Id = model.Id;
                g.Answer = model.Answer == null ? "" : model.Answer;
                g.IsCorrect = model.IsCorrect;
                g.TestQuestion = entity;
            };
        }

        /// <summary> Существует ли соответствующая запись в БД? </summary>
        /// <remarks> При реализации - просто проверить ключ, в базу лазить НЕ НАДО </remarks>
        protected override bool ExistsInDatabase(AnswerVariantModel model)
        {
            return model.Id != 0;
        }

        /// <summary> При реализации возвращает массив первичных ключей сущности </summary>
        protected override object[] GetEntityKey(AnswerVariantModel model)
        {
            return new object[] { model.Id };
        }
    }
}
