using Core.Entities;
using Core.Entities.Concrete;

namespace Entities.Concrete
{
    public class Card:IEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string CardNumber { get; set; }
        public string HolderName { get; set; }
        public string Cvc { get; set; }
        public string ExpireMonth { get; set; }
        public string ExpireYear { get; set; }

    }
}
