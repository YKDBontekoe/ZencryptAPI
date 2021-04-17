using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Neo
{
    public class NeoEntity
    {
        public int id { get; set; }
        public string EntityId { get; set; } 
        public string entityType { get; set; }
    }
}
