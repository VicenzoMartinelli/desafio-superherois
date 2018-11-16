using Core.Entity;
using Core.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SuperheroWeb.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SuperHeroes_Softfocus.Models
{
  [Route("[controller]")]
  public class ApiController : Controller
  {
    private readonly IRepository _repository;
    private readonly IRepositorySuperHero _repositoryHero;
    private readonly IHostingEnvironment _appEnvironment;

    public ApiController(IRepository repository, IRepositorySuperHero repositoryHero, IHostingEnvironment appEnvironment)
    {
      _repository = repository;
      _repositoryHero = repositoryHero;
      _appEnvironment = appEnvironment;
    }

    [HttpGet("getbyid/{id}")]
    public async Task<IActionResult> GetById(string id)
    {
      try
      {
        var result = await _repository.GetByIdAsync<SuperHero>(id);
        var image  = "data:image/jpeg;base64," + Convert.ToBase64String(System.IO.File.ReadAllBytes(Path.Combine(_appEnvironment.WebRootPath, @"images/superheroes") + $@"/{id + ".jpg"}"));
        #if DEBUG
        image  = "data:image/jpeg;base64," + Convert.ToBase64String(System.IO.File.ReadAllBytes(Path.Combine(_appEnvironment.WebRootPath, @"images\superheroes") + $@"\{id + ".jpg"}"));
        #endif
        return Ok(new
        {
          result.Id,
          result.Description,
          result.Name,
          result.IsFavorite,
          Image = image
        });
      }
      catch (Exception)
      {
        return StatusCode(StatusCodes.Status500InternalServerError);
      }
    }

    [HttpGet("{name}")]
    [Route("")]
    [Route("get")]
    [Route("get/{name}")]
    public async Task<IActionResult> Get(string name)
    {
      try
      {
        var @return = string.IsNullOrEmpty(name) ? await _repository.GetAll<SuperHero>() : await _repositoryHero.GetByName(name);

        var returnWithImg = new List<object>();
        @return.ToList().ForEach(x => 
        {
          var image = default(string);
          try
          {
            image = "data:image/jpeg;base64," + Convert.ToBase64String(System.IO.File.ReadAllBytes(Path.Combine(_appEnvironment.WebRootPath, @"images/superheroes") + $@"/{x.Id + ".jpg"}"));
            #if DEBUG
            image = "data:image/jpeg;base64," + Convert.ToBase64String(System.IO.File.ReadAllBytes(Path.Combine(_appEnvironment.WebRootPath, @"images\superheroes") + $@"\{x.Id + ".jpg"}"));
            #endif
          }
          catch (Exception)
          {
          }

          returnWithImg.Add(new
          {
            x.Id,
            x.IsFavorite,
            x.Name,
            x.Description,
            Image = image
          });
        });

        return Ok(returnWithImg);
      }
      catch (Exception)
      {
        return StatusCode(StatusCodes.Status500InternalServerError);
      }
    }

    [HttpDelete("deletehero/{id}")]
    public async Task<IActionResult> DeleteHero(string id)
    {
      try
      {
        var Status = await this._repository.DeleteAsync<SuperHero>(id);

        System.IO.File.Delete(Path.Combine(_appEnvironment.WebRootPath, @"images/superheroes") + $@"/{id + ".jpg"}");

        #if DEBUG
        System.IO.File.Delete(Path.Combine(_appEnvironment.WebRootPath, @"images\superheroes") + $@"\{id + ".jpg"}");
        #endif
        return Ok();
      }
      catch(Exception e)
      {
        return StatusCode(StatusCodes.Status500InternalServerError);
      }
      
    }

    [HttpPost("newhero")]
    public async Task<IActionResult> NewHero([FromBody] SuperHeroDTO model)
    {
      if (!ModelState.IsValid)
        return StatusCode(StatusCodes.Status400BadRequest);

      try
      {
        var heroSave = new SuperHero()
        {
          Description = model.Description,
          Name = model.Name
        };

        await _repository.SaveOrUpdateAsync(heroSave, model.Id);

        var content = model.Picture.Split(",")[1];

        var path = Path.Combine(_appEnvironment.WebRootPath, @"images/superheroes") + $@"/{heroSave.Id + ".jpg"}";
        #if DEBUG
        path = Path.Combine(_appEnvironment.WebRootPath, @"images\superheroes") + $@"\{heroSave.Id + ".jpg"}";
        #endif
        System.IO.File.Delete(path);

        using (FileStream SourceStream = System.IO.File.Open(path, FileMode.OpenOrCreate))
        {
          SourceStream.Seek(0, SeekOrigin.End);
          await SourceStream.WriteAsync(Convert.FromBase64String(content), 0, Convert.FromBase64String(content).Length);
        }

        return Ok();
      }
      catch (Exception)
      {
        return StatusCode(StatusCodes.Status500InternalServerError);
      }
    }


    [HttpPut("setfavorite/{id}")]
    public async Task<IActionResult> SetFavorite(string id)
    {
      try
      {
        var super = await _repository.GetByIdAsync<SuperHero>(id);
        super.IsFavorite = !super.IsFavorite;
        await _repository.UpdateAsync(super, id);

        return Ok();
      }
      catch (Exception)
      {
        return StatusCode(StatusCodes.Status500InternalServerError);
      }
    }
  }
}