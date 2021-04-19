using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Domain.Entities.Neo
{
    public class NeoEntity
    {
        [JsonProperty(PropertyName = "EntityId")]
        public string EntityId { get; set; }
        [JsonProperty(PropertyName = "entityType")]
        public string entityType { get; set; }
    }
}
