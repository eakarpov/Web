using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphLabs.DomainModel;
using GraphLabs.Site.Models.AnswerVariant;
using GraphLabs.Site.Models.Category;
using GraphLabs.Site.Models.Infrastructure;
using GraphLabs.Site.Models.TestPool;

namespace GraphLabs.Site.Models.TestQuestion
{
    public class EditTestQuestionModel : IEntityBasedModel<DomainModel.TestQuestion>
    {
        public long Id { get; set; }

        public string Question { get; set; }

        public CategoryModel Category { get; set; }

        public AnswerVariantModel[] AnswerVariants { get; set; }
    }
}
