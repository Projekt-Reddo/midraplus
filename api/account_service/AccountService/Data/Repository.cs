using MongoDB.Bson;
using MongoDB.Driver;

namespace AccountService.Data
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class
    {
        /// <summary>
        /// Get all documents match filter
        /// </summary>
        /// <param name="filter">Filter builder for filter element</param>
        /// <param name="sort">
        ///     <para>Bson document for sort (1: increase, -1: decrease)</para>
        ///     <para> Example: new BsonDocument { { "fieldName", 1 } }</para>
        /// </param>
        /// <param name="lookup">
        ///  <para>Bson document for lookup</para>
        ///  <para> Example: 
        ///  new BsonDocument{
        ///     { "from", "target_document_name" },
        ///     { "localField", "field_for_comparision(Note: using _id not Id)" },
        ///     { "foreignField", "foreign_key" },
        ///     { "as", "joined_document_field" }
        ///  }
        /// </para>
        /// </param>
        /// <param name="limit">Number of documents to get</param>
        /// <param name="skip">Number of documents to skip</param>
        /// <returns>Total match filter count and List of documents</returns>
        Task<(long total, IEnumerable<TEntity> entities)> FindManyAsync(FilterDefinition<TEntity> filter = default(FilterDefinition<TEntity>)!, BsonDocument? sort = null!, BsonDocument? lookup = null!, int? limit = null!, int? skip = null!);

        /// <summary>
        /// Get a document by fitler
        /// </summary>
        /// <param name="filter">Bson filter</param>
        /// <returns>Fit condition document</returns>
        Task<TEntity> FindOneAsync(FilterDefinition<TEntity> filter = default(FilterDefinition<TEntity>)!);

        /// <summary>
        /// Add new document to selected collection
        /// </summary>
        /// <param name="entity">Entity to add</param>
        /// <returns>New entity if created</returns>
        Task<TEntity> AddOneAsync(TEntity entity);

        /// <summary>
        /// Update a document in selected collection with new value
        /// </summary>
        /// <param name="id">Document id</param>
        /// <param name="entity">New document</param>
        /// <returns>true(updated) / false(not update)</returns>
        Task<bool> UpdateOneAsync(string id, TEntity entity);

        /// <summary>
        /// Delete a document in selected collection by id
        /// </summary>
        /// <param name="id">Id to delete</param>
        /// <returns>true(deleted) / false(not delete)</returns>
        Task<bool> DeleteOneAsync(string id);
    }

    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly IMongoCollection<TEntity> _collection;
        protected readonly IMongoDatabase _database;

        public Repository(IMongoContext context)
        {
            _database = context.Database;
            _collection = _database.GetCollection<TEntity>(typeof(TEntity).Name.ToLower());
        }

        public virtual async Task<TEntity> AddOneAsync(TEntity entity)
        {
            await _collection.InsertOneAsync(entity);
            return entity;
        }

        public virtual async Task<(long total, IEnumerable<TEntity> entities)> FindManyAsync(FilterDefinition<TEntity> filter = null!, BsonDocument? sort = null!, BsonDocument? lookup = null!, int? limit = null!, int? skip = null!)
        {

            var query = _collection.Aggregate().Match(filter is null ? Builders<TEntity>.Filter.Empty : filter);

            if (skip is not null)
            {
                query = query.Skip(skip.Value);
            }

            if (limit is not null)
            {
                query = query.Limit(limit.Value);
            }

            if (lookup is not null)
            {
                query = query.AppendStage<TEntity>(new BsonDocument
                {
                    {
                        "$lookup", lookup
                    }
                });
            }

            if (sort is not null)
            {
                query = query.Sort(sort);
            }

            long total = await _collection.CountDocumentsAsync(filter is null ? Builders<TEntity>.Filter.Empty : filter);
            var entities = await query.ToListAsync();
            return (total, entities);
        }

        public virtual async Task<bool> UpdateOneAsync(string id, TEntity entity)
        {
            var rs = await _collection.ReplaceOneAsync(Builders<TEntity>.Filter.Eq("Id", id), entity);
            return rs.ModifiedCount > 0 ? true : false;
        }

        public virtual async Task<bool> DeleteOneAsync(string id)
        {
            var rs = await _collection.DeleteOneAsync(Builders<TEntity>.Filter.Eq("Id", id));
            return rs.DeletedCount > 0 ? true : false;
        }

        public virtual async Task<TEntity> FindOneAsync(FilterDefinition<TEntity> filter = null!)
        {
            var entity = await _collection.Find(filter is null ? Builders<TEntity>.Filter.Empty : filter).FirstOrDefaultAsync();
            return entity;
        }

        /// <summary>
        /// Release unmanage resources
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}