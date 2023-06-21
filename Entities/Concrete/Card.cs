using Core.Entities;
using Core.Entities.Concrete;

namespace Entities.Concrete
{
    public class Card:IEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string CardNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CVC { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }

    }
}
