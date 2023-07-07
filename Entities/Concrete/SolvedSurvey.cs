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
    public class SolvedSurvey:IMongoDBEntity
    {
        [BsonId, BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public int UserId { get; set; }
        public string SurveyId { get; set; }
        public IEnumerable<QuestionAnswer> QuestionsAnswers { get; set; }
    }
    public class QuestionAnswer 
    { 
        public string QuestionDescription { get; set; }
        public IEnumerable<SelectedOption> SelectedAnswers { get; set; }
    }
    public class SelectedOption
    { 
        public string SelectedOptionDescription { get; set; }
    }
}
