using GraphLabs.DomainModel;
using GraphLabs.Site.Models.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphLabs.Site.Models.AnswerVariant
{
    public class AnswerVariantModel : IEntityBasedModel<DomainModel.AnswerVariant>
    {
        public long Id { get; set; }
        public string Answer { get; set; }
        public bool IsCorrect { get; set; }
        public long TestQuestion { get; set; }
    }
}
