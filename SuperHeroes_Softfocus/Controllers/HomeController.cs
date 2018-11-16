using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SuperHeroes_Softfocus.Models;
using Core.Repository;
using Core.Entity;
using SuperheroWeb.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Net.Http.Headers;

namespace SuperHeroes_Softfocus.Controllers
{
  public class HomeController : Controller
  {
    private readonly IRepository _repository;
    private readonly IRepositorySuperHero _repositoryHero;
    private readonly IHostingEnvironment _appEnvironment;

    public HomeController(IRepository repository, IRepositorySuperHero repositoryHero, IHostingEnvironment appEnvironment)
    {
      _repository = repository;
      _repositoryHero = repositoryHero;
      _appEnvironment = appEnvironment;
    }

    public async Task <IActionResult> Index(string filtrer, int? page)
    {
      var heroes = string.IsNullOrEmpty(filtrer) ? await _repository.GetAll<SuperHero>() : await _repositoryHero.GetByName(filtrer);

      var model = new HomePageModel()
      {
        Heroes = heroes,
        NumPages = (int) Math.Ceiling((double) heroes.Count / 12),
        Page = page ?? 1,
        Filtro = filtrer
      };

      return View(model);
    }

    public async Task<IActionResult> Favorites()
    {
      var heroes = await _repositoryHero.GetFavorites();
      
      return View(heroes);
    }

    public async Task<IActionResult> Save(SuperheroDTO superhero)
    {
      if (!ModelState.IsValid)
        return RedirectToAction("Error");

      try
      {
        var heroSave = string.IsNullOrEmpty(superhero.Id) ? new SuperHero() : await _repository.GetByIdAsync<SuperHero>(superhero.Id);
        heroSave.Description = superhero.Description;
        heroSave.Name = superhero.Name;

        await _repository.SaveOrUpdateAsync(heroSave, superhero.Id);

        var content = superhero.Picture.Split(",")[1];
        var fileName = heroSave.Id;

        var path = Path.Combine(_appEnvironment.WebRootPath, @"images\superheroes") + $@"\{fileName + ".jpg"}";

        System.IO.File.Delete(path);

        using (FileStream SourceStream = System.IO.File.Open(path, FileMode.OpenOrCreate))
        {
          SourceStream.Seek(0, SeekOrigin.End);
          await SourceStream.WriteAsync(Convert.FromBase64String(content), 0, Convert.FromBase64String(content).Length);
        }

        return RedirectToAction("Index");
      }
      catch (Exception)
      {
        return RedirectToAction("Error");
      }
    }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
  }
}
