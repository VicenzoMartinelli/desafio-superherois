using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperHeroes_Softfocus.Config
{
  public interface IDBContext
  {
    IMongoDatabase GetDatabase();
  }
}
