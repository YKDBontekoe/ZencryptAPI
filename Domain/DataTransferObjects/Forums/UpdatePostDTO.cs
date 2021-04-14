using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DataTransferObjects.Forums
{
    public class UpdatePostDTO
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public string EditedByUserToken { get; set; }   
    }
}
