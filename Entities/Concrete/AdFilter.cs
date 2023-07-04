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
    public class AdFilter:IMongoDBEntity
    {
        [BsonId, BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string AdId { get; set; }
        public int MinAge { get; set; }
        public int MaxAge { get; set; }
        public int GenderId { get; set; }
    }
}
