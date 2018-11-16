using System.Linq;
using MongoDB.Driver;
using Microsoft.Extensions.Configuration;
using System;
using MongoDB.Bson;
using SuperHeroes_Softfocus.Config;

namespace Api
{
  public class DBContext : IDBContext
  {
    private IConfiguration _configuration;
    private IMongoDatabase db;

    public DBContext(IConfiguration config)
    {
      _configuration = config;
    }

    public IMongoDatabase GetDatabase()
    {
      if(db == null)
      {
        var mongoClient = new MongoClient(_configuration.GetConnectionString("Connection"));
        db = mongoClient.GetDatabase("superheroes");

        bool isAlive = db.RunCommandAsync((Command<BsonDocument>)"{ping:1}").Wait(1000);

        if (!isAlive)
          throw new Exception();
      }

      return db;
    }
  }
}