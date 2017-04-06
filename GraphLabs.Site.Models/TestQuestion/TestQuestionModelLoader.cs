using GraphLabs.DomainModel;
using GraphLabs.Site.Models.Infrastructure;
using System.Diagnostics.Contracts;

namespace GraphLabs.Site.Models.TestQuestion
{
    internal sealed class TestQuestionModelLoader : AbstractModelLoader<TestQuestionModel, DomainModel.TestQuestion>
    {
        /// <summary> Загрузчик моделей вопросов </summary>
        public TestQuestionModelLoader(IEntityQuery query) : base(query) { }

        /// <summary> Загрузить по сущности-прототипу </summary>
        public override TestQuestionModel Load(DomainModel.TestQuestion question)
        {
            Contract.Requires(question != null);

            var model = new TestQuestionModel
            {
                Id = question.Id,
                Question = question.Question,
                Category = question.Category
            };

            return model;
        }
    }
}
