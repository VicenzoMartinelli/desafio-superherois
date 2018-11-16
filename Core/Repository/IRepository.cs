using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repository
{
  public interface IRepository
  {
    Task<T> GetByIdAsync<T>(string id) where T : class;
    Task<List<T>> GetAll<T>() where T : class;
    Task<List<T>> GetByFilter<T>(FilterDefinition<T> filter) where T : class;
    Task<T> AddAsync<T>(T source) where T : class;
    Task<T> UpdateAsync<T>(T source, string id) where T : class;
    Task<T> SaveOrUpdateAsync<T>(T source, string id) where T : class;
    Task<bool> DeleteAsync<T>(string id) where T : class;
  }
}
