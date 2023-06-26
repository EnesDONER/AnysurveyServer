using Core.DataAccess.MongoOptions;
using DataAccess.Abstract;
using DataAccess.Context;
using DataAccess.Repository;
using Entities.Concrete;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.MongoDB
{
    public class MWatchedAdDal : MongoEntityRepositoryBase<WatchedAd>, IWatchedAdDal
    {
        private MongoDBContext _context;
        private IMongoCollection<WatchedAd> _collection;
        public MWatchedAdDal(IOptions<MongoSettings> options) : base(options)
        {
            _context = new MongoDBContext(options);
            _collection = _context.GetCollection<WatchedAd>();
        }
    }
}
