using System.Collections.Generic;
using System.Collections.ObjectModel;
using GraphLabs.DomainModel;
using GraphLabs.Site.Models.Infrastructure;
using System.Diagnostics.Contracts;
using System.Linq;
using GraphLabs.Site.Models.AnswerVariant;
using GraphLabs.Site.Models.Category;
using Microsoft.Practices.ObjectBuilder2;

namespace GraphLabs.Site.Models.TestQuestion
{
    internal sealed class EditTestQuestionModelLoader : AbstractModelLoader<EditTestQuestionModel, DomainModel.TestQuestion>
    {
        /// <summary> Загрузчик моделей вопросов </summary>
        public EditTestQuestionModelLoader(IEntityQuery query) : base(query) { }

        /// <summary> Загрузить по сущности-прототипу </summary>
        public override EditTestQuestionModel Load(DomainModel.TestQuestion question)
        {
            Contract.Requires(question != null);
            var modelAnswer = new AnswerVariantModel[question.AnswerVariants.Count];
            for (int i = 0; i < question.AnswerVariants.Count; i++)
            {
                modelAnswer[i] = new AnswerVariantModel
                {
                    Id = question.AnswerVariants.ElementAt(i).Id,
                    Answer = question.AnswerVariants.ElementAt(i).Answer,
                    IsCorrect = question.AnswerVariants.ElementAt(i).IsCorrect,
                    TestQuestion = question.AnswerVariants.ElementAt(i).TestQuestion.Id
                };
            }
            var modelCategory = new CategoryModel
            {
                Id = question.Category.Id,
                Name = question.Category.Name
            };
            var model = new EditTestQuestionModel
            {
                Id = question.Id,
                Question = question.Question,
                Category = modelCategory,
                AnswerVariants = modelAnswer,
            };

            return model;
        }
    }
}
