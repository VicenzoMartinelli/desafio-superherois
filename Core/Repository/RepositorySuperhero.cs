using Core.Entity;
using MongoDB.Driver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repository
{
  public class RepositorySuperHero : IRepositorySuperHero
  {
    private readonly IRepository _repository;

    public RepositorySuperHero(IRepository repository)
    {
      _repository = repository;
    }

    public async Task<IList<SuperHero>> GetByName(string filtrer)
    {
      var filterObj = Builders<SuperHero>.Filter.Eq(x => x.Name, filtrer);

      return await _repository.GetByFilter(filterObj);
    }

    public async Task<IList<SuperHero>> GetFavorites()
    {
      var filterObj = Builders<SuperHero>.Filter.Where(x => x.IsFavorite);

      return await _repository.GetByFilter(filterObj);
    }
  }
}
