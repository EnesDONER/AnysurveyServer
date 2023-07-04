using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Gender:IEntity
    {
        public int GenderId { get; set; }
        public string GenderName { get; set; }
    }
}
