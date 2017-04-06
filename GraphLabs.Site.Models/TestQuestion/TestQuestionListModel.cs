using GraphLabs.DomainModel;
using GraphLabs.Site.Models.Infrastructure;
using System.Linq;
using GraphLabs.Site.Models.TestQuestion;

namespace GraphLabs.Site.Models.TestQuestion
{
    public sealed class TestQuestionListModel : ListModelBase<TestQuestionModel>
    {
        private readonly IEntityQuery _query;
        private readonly IEntityBasedModelLoader<TestQuestionModel, DomainModel.TestQuestion> _modelLoader;

        /// <summary> Модель списка вопросов </summary>
        public TestQuestionListModel(IEntityQuery query, IEntityBasedModelLoader<TestQuestionModel, DomainModel.TestQuestion> modelLoader)
        {
            _query = query;
            _modelLoader = modelLoader;
        }

        /// <summary> Загружает вопросы </summary>
        protected override TestQuestionModel[] LoadItems()
        {
            return _query.OfEntities<DomainModel.TestQuestion>()
                .ToArray()
                .Select(l => _modelLoader.Load(l))
                .OrderBy(i => i.Category.Name)
                .ToArray();
        }
    }
}
