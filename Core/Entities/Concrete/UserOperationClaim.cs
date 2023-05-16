using MongoDB.Bson;

namespace Core.Entities.Concrete
{
    public class UserOperationClaim 
    {
        public ObjectId Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public int UserId { get; set; }
        public int OperationClaimId { get; set; }
    }
}