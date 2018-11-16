using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Core.Entity;
using MongoDB.Bson;

namespace Core.Repository
{
  public class Repository : IRepository
  {
    private IMongoDatabase db;

    public Repository(IMongoDatabase db)
    {
      this.db = db;
    }

    public async Task<T> GetByIdAsync<T>(string id) where T : class
    {
      if (!id.Length.Equals(24))
        return default(T);

      var filter = Builders<T>.Filter.Eq("_id", new ObjectId(id));

      var x = await db.GetCollection<T>(typeof(T).Name)
          .Find(filter).FirstOrDefaultAsync();
      return x;
    }

    public async Task<List<T>> GetAll<T>() where T : class
    {
      var lst = await db.GetCollection<T>(typeof(T).Name).FindAsync("{}");
      return lst.ToList();
    }

    public async Task<List<T>> GetByFilter<T>(FilterDefinition<T> filter) where T : class
    {
      var lst = await db.GetCollection<T>(typeof(T).Name).FindAsync(filter);
      return lst.ToList();
    }

    public async Task<T> AddAsync<T>(T source) where T : class
    {
      var collection = db.GetCollection<T>(typeof(T).Name);

      try
      {
        await collection.InsertOneAsync(source);

        return source;
      }
      catch (Exception e)
      {
        throw e;
      }

    }

    public async Task<T> UpdateAsync<T>(T source, string id) where T : class
    {
      var filter = Builders<T>.Filter.Eq("_id", new ObjectId(id));

      await db.GetCollection<T>(typeof(T).Name).ReplaceOneAsync(filter, source);
      return source;
    }

    public async Task<bool> DeleteAsync<T>(string id) where T : class
    {
      try
      {
        var filter = Builders<T>.Filter.Eq("_id", new ObjectId(id));
        await db.GetCollection<T>(typeof(T).Name).DeleteOneAsync(filter);

        return true;
      }
      catch (Exception)
      {
        return false;
      }
    }

    public Task<T> SaveOrUpdateAsync<T>(T source, string id) where T : class
    {
      if (string.IsNullOrEmpty(id))
        return AddAsync(source);
      else
        return UpdateAsync(source, id);
    }
  }
}
