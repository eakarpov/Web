using GraphLabs.DomainModel;
using GraphLabs.Site.Models.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphLabs.Site.Models.TestQuestion
{
    public class TestQuestionModel : IEntityBasedModel<DomainModel.TestQuestion>
    {
        public long Id { get; set; }
        public string Question { get; set; }
        public virtual DomainModel.Category Category { get; set; }
    }
}
