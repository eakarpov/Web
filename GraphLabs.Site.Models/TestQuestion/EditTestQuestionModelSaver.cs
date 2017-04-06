using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphLabs.Site.Models.Infrastructure;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Contexts;
using GraphLabs.DomainModel.Extensions;
using GraphLabs.Site.Core.OperationContext;
using GraphLabs.Site.Models.AnswerVariant;
using GraphLabs.Site.Models.TestPoolEntry;
using GraphLabs.Site.Models.TestQuestion;
using Microsoft.Practices.ObjectBuilder2;

namespace GraphLabs.Site.Models.TestQuestion
{
    internal sealed class EditTestQuestionModelSaver : AbstractModelSaver<EditTestQuestionModel, DomainModel.TestQuestion>
    {

        private readonly IOperationContextFactory<IGraphLabsContext> _operationContextFactory;
        private readonly IEntityBasedModelSaver<AnswerVariantModel, DomainModel.AnswerVariant> _modelSaver;

        public EditTestQuestionModelSaver(
            IOperationContextFactory<IGraphLabsContext> operationContextFactory,
            IEntityBasedModelSaver<AnswerVariantModel, DomainModel.AnswerVariant> modelSaver
            ) : base(operationContextFactory)
        {
            _operationContextFactory = operationContextFactory;
            _modelSaver = modelSaver;
        }

        protected override Action<DomainModel.TestQuestion> GetEntityInitializer(EditTestQuestionModel model, IEntityQuery query)
        {
            var entity = new Collection<DomainModel.AnswerVariant>();
            var entityCategory = query.Get<DomainModel.Category>(model.Category.Id); 
            model.AnswerVariants.ForEach(t =>
            {
                var entityAnswer = query.Get<DomainModel.AnswerVariant>(t.Id);
                entityAnswer.Answer = t.Answer;
                entityAnswer.IsCorrect = t.IsCorrect;
                entity.Add(entityAnswer);
            });
            return g =>
            {
                g.Id = model.Id;
                g.Category = entityCategory;
                g.AnswerVariants = entity;
                g.Question = model.Question;
            };
        }

        public Action<DomainModel.AnswerVariant> GetEntityInitializer(AnswerVariantModel model, IEntityQuery query)
        {
            var entity = query.Get<DomainModel.TestQuestion>(model.TestQuestion);
            return g =>
            {
                g.Id = model.Id;
                g.Answer = model.Answer;
                g.IsCorrect = model.IsCorrect;
                g.TestQuestion = entity;
            };
        }

        /// <summary> Существует ли соответствующая запись в БД? </summary>
        /// <remarks> При реализации - просто проверить ключ, в базу лазить НЕ НАДО </remarks>
        protected override bool ExistsInDatabase(EditTestQuestionModel model)
        {
            return model.Id != 0;
        }

        /// <summary> При реализации возвращает массив первичных ключей сущности </summary>
        protected override object[] GetEntityKey(EditTestQuestionModel model)
        {
            return new object[] { model.Id };
        }
    }
}
