using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entity;

namespace Core.Repository
{
  public interface IRepositorySuperHero
  {
    Task<IList<SuperHero>> GetByName(string filtrer);
    Task<IList<SuperHero>> GetFavorites();
  }
}