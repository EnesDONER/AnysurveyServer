
using Core.DataAccess.MongoOptions;
using DataAccess.Abstract;
using DataAccess.Context;
using DataAccess.Repository;
using Entities.Concrete;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DataAccess.Concrete.MongoDB
{
    public class MSurveyDal : MongoEntityRepositoryBase<Survey>, ISurveyDal
    {
        private readonly MongoDBContext _context;
        private readonly IMongoCollection<Survey> _collection;
        public MSurveyDal(IOptions<MongoSettings> options) : base(options)
        {
            _context = new MongoDBContext(options);
            _collection = _context.GetCollection<Survey>();
        }
    }
}
