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
    public class MAdDal : MongoEntityRepositoryBase<Ad>, IAdDal
    {
        private readonly MongoDBContext _context;
        private readonly IMongoCollection<Ad> _collection;
        public MAdDal(IOptions<MongoSettings> options) : base(options)
        {
            _context = new MongoDBContext(options);
            _collection = _context.GetCollection<Ad>();
        }
    }
}
