using Core.Entity;
using Core.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
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

    [HttpGet("setfavorite/{id}")]
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