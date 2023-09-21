using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class EmailDto:IDto
    {
        public string ConsumerUserEmail{ get; set; }
        public string Body { get; set; }
        public string Subject { get; set; }

    }
}
