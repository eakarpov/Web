using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphLabs.Site.Models.Infrastructure;

namespace GraphLabs.Site.Models.Category
{
    public class CategoryModel : IEntityBasedModel<DomainModel.Category>
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
}
