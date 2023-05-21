using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameStore.API.Entities;

namespace GameStore.API.Repositories;

public class InMemGamesRepository : IGamesRepository
{
    private readonly List<Game> games = new(){
    new Game(){
        Id=1,
        Name="Street Fighter II",
        Price=19.99M,
        Genre="Fighting",
        ReleaseDate=new DateTime(1991,2,1),
        ImageUri="http://placehold.co/100"
    },
    new Game(){
        Id=2,
        Name="Final Fantasy XVI",
        Price=49.99M,
        Genre="Roleplaying",
        ReleaseDate=new DateTime(2010,9,30),
        ImageUri="http://placehold.co/100"
    },
    new Game(){
        Id=3,
        Name="FIFA23",
        Price=69.99M,
        Genre="Sports",
        ReleaseDate=new DateTime(2022,9,27),
        ImageUri="http://placehold.co/100"
    }
    };
    public async Task<IEnumerable<Game>> GetAllAsync(int pageNumber, int pageSize, string? filter)
    {
        var skipCount = (pageNumber-1)*pageSize;
        return await Task.FromResult(FilterGames(filter).OrderBy(order => order.Id).Skip(skipCount).Take(pageSize));
    }

    public async Task<Game?> GetAsync(int id)
    {
        return await Task.FromResult(games.Find(game => game.Id == id));
    }

    public async Task CreateAsync(Game game)
    {
        game.Id = games.Max(game => game.Id) + 1;
        games.Add(game);
        await Task.CompletedTask;
    }

    public async Task UpdateAsync(Game updatedGame)
    {
        var index = games.FindIndex(game => game.Id == updatedGame.Id);
        games[index] = updatedGame;
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(int id)
    {
        var index = games.FindIndex(game => game.Id == id);
        games.RemoveAt(index);
        await Task.CompletedTask;
    }
    public async Task<int> CountAsync(string? filter)
    {
        return await Task.FromResult(FilterGames(filter).Count());
    }
    private IEnumerable<Game> FilterGames(string? filter)
    {
        if(string.IsNullOrEmpty(filter))
        {
            return games;
        }
        return games.Where(game => game.Name.Contains(filter) || game.Genre.Contains(filter));
    }
}