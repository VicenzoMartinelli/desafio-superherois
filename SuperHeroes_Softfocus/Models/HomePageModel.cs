using Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperHeroes_Softfocus.Models
{
  public class HomePageModel
  {
    public IList<SuperHero> Heroes { get; set; }
    public const int HeroesPage = 12;
    public int NumPages { get; set; }
    public int Page { get; set; }
    public string Filtro { get; set; }

  }
}
