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
    public class MAdFilterDal : MongoEntityRepositoryBase<AdFilter>, IAdFilterDal
    {
        private MongoDBContext _context;
        private IMongoCollection<AdFilter> _collection;
        public MAdFilterDal(IOptions<MongoSettings> options) : base(options)
        {
            _context = new MongoDBContext(options);
            _collection = _context.GetCollection<AdFilter>();
        }
    }
}
