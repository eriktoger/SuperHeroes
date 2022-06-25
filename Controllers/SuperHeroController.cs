using System;
using Microsoft.AspNetCore.Mvc;
using MongoExample.Services;
using MongoExample.Models;

namespace SuperHeroAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class SuperHeroController : ControllerBase
{
    private readonly MongoDBService _mongoDBService;
    private readonly ILogger<SuperHeroController> _logger;

    public SuperHeroController(ILogger<SuperHeroController> logger, MongoDBService mongoDBService)
    {
        _mongoDBService = mongoDBService;
        _logger = logger;
    }

    [HttpGet(Name = "GetAllSuperHeros")]
    public async Task<IActionResult> GetAll()
    {
        var heroes = await _mongoDBService.GetAll();
        return Ok(heroes);
    }


    [HttpPost(Name = "PostSuperHero")]
    public async Task<IActionResult> Add(SuperHero hero)
    {
        await _mongoDBService.Create(hero);
        var heroes = await _mongoDBService.GetAll();
        return Ok(heroes);
    }

    [HttpGet("{id}", Name = "GetSuperHero")]
    public async Task<IActionResult> GetOne(string id)
    {
        var hero = await _mongoDBService.GetOne(id);
        if (hero == null)
        {
            return NotFound("Hero not found");
        }
        return Ok(hero);
    }


    [HttpPut(Name = "PutSuperHero")]
    public async Task<IActionResult> Update(SuperHero hero)
    {
        var oldHero = await _mongoDBService.GetOne(hero.Id);

        if (oldHero == null)
        {
            return NotFound("Hero not found");
        }
        await _mongoDBService.Update(hero);
        var heroes = await _mongoDBService.GetAll();
        return Ok(heroes);
    }

    [HttpDelete(Name = "DeleteSuperHero")]
    public async Task<IActionResult> Remove(string id)
    {
        var oldHero = await _mongoDBService.GetOne(id);

        if (oldHero == null)
        {
            return NotFound("Hero not found");
        }
        await _mongoDBService.Delete(id);
        var heroes = await _mongoDBService.GetAll();
        return Ok(heroes);
    }

}
