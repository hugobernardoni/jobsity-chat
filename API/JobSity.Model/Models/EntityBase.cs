using JobSity.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSity.Model.Models
{
    public class EntityBase : IEntityBase
    {
        public Guid Id { get; set; }
    }
}
