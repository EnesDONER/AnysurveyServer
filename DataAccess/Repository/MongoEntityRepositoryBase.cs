using Core.DataAccess;
using Core.DataAccess.MongoOptions;
using Core.Entities;
using DataAccess.Context;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using SharpCompress.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class MongoEntityRepositoryBase<TEntity> : IEntityRepository<TEntity>
       where TEntity : class, IMongoDBEntity, new()

    {
        private readonly MongoDBContext _context;

        private readonly IMongoCollection<TEntity> _collection;

        public MongoEntityRepositoryBase(IOptions<MongoSettings> options)
        {
            _context = new MongoDBContext(options);
            _collection = _context.GetCollection<TEntity>();
        }

        public void Add(TEntity entity)
        {
            _collection.InsertOne(entity);
        }

        public void Delete(TEntity entity)
        {
            var objectId = ObjectId.Parse(entity.Id);
            var filter = Builders<TEntity>.Filter.Eq("_id", objectId);
            _collection.DeleteOne(filter);
        }

        public TEntity Get(Expression<Func<TEntity, bool>> filter)
        {
            return _collection.Find(filter).FirstOrDefault();
        }

        public List<TEntity> GetAll(Expression<Func<TEntity, bool>> filter = null)
        {
            var query = filter == null ? _collection.AsQueryable() : _collection.AsQueryable().Where(filter);
            return query.ToList();
        }

        public void Update(TEntity entity)
        {
            var objectId = ObjectId.Parse(entity.Id);
            var filter = Builders<TEntity>.Filter.Eq("_id", objectId);
            _collection.ReplaceOne(filter, entity);
        }
    }
}
