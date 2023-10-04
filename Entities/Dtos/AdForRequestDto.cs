using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public  class AdForRequestDto
    {
        public int OwnerUserId { get; set; }
        public string CompanyName { get; set; }
        public string Description { get; set; }
        public string VideoUrl { get; set; }
        public string ImageContainerName { get; set; }
        public IFormFile VideoImage { get; set; }
    }
}
