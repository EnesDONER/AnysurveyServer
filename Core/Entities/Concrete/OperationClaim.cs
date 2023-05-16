using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Core.Entities.Concrete
{
    public class OperationClaim 
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public string Name { get; set; }
    }
}