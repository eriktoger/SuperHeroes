
using MongoExample.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;

namespace MongoExample.Services;

public class MongoDBService
{

    private readonly IMongoCollection<SuperHero> _superHeroCollection;

    public MongoDBService(IOptions<MongoDBSettings> mongoDBSettings)
    {
        MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
        IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
        _superHeroCollection = database.GetCollection<SuperHero>(mongoDBSettings.Value.CollectionName);
    }

    public async Task<List<SuperHero>> GetAll()
    {
        return await _superHeroCollection.Find(new BsonDocument()).ToListAsync();

    }
    public async Task Create(SuperHero hero)
    {
        await _superHeroCollection.InsertOneAsync(hero);
        return;

    }

    public async Task<SuperHero> GetOne(string id)
    {
        return await _superHeroCollection.Find(hero => hero.Id == id).SingleAsync();
    }

    public async Task Update(SuperHero hero)
    {
        var updateResult = await _superHeroCollection.ReplaceOneAsync(
     h => h.Id == hero.Id,
    hero
    );
        return;
    }

    public async Task Delete(string id)
    {
        await _superHeroCollection.DeleteOneAsync(hero => hero.Id == id);
    }


}
