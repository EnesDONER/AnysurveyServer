using Core.Entities;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Ad : IMongoDBEntity
    {
        [BsonId, BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public int OwnerUserId { get; set; }
        public string CompanyName { get; set; }
        public string Description { get; set; }
        public string VideoURL { get; set; }

    }
}
