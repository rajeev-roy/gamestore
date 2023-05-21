using System.Diagnostics;
using GameStore.API.Authorization;
using GameStore.API.Dtos;
using GameStore.API.Entities;
using GameStore.API.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;

namespace GameStore.API.Endpoints;

public static class GamesEndpoints
{
    const string GetGameV1EndpointName = "GetGameV1";
    const string GetGameV2EndpointName = "GetGameV2";
    public static RouteGroupBuilder MapGamesEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.NewVersionedApi()
            .MapGroup("/games")
            .HasApiVersion(1.0)
            .HasApiVersion(2.0)
            .WithParameterValidation()
            .WithOpenApi()
            .WithTags("Games");

        group.MapGet("/", async (IGamesRepository repository, 
        ILoggerFactory loggerFactory, 
        [AsParameters] GetGamesDtoV1 request,
        HttpContext http) =>
        {
            var totalCount = await repository.CountAsync(request.filter);
            http.Response.AddPaginationHeader(totalCount,request.pageSize);
            return Results.Ok((await repository.GetAllAsync(request.pageNumber, 
            request.pageSize,request.filter)).Select(game => game.AsDtosV1()));
        })
        .MapToApiVersion(1.0)
        .WithSummary("Gets all games")
        .WithDescription("Gets all available games and allows filtering and pagination");

        group.MapGet("/{id}", async Task<Results<Ok<GameDtoV1>,NotFound>> (IGamesRepository repository, int id) =>
        {
            Game? game = await repository.GetAsync(id);
            return game is not null ? TypedResults.Ok(game.AsDtosV1()) : TypedResults.NotFound();
        })
        .WithName(GetGameV1EndpointName)
        .RequireAuthorization(Policies.Read_Access)
        .MapToApiVersion(1.0)
        .WithSummary("Get a game by Id")
        .WithDescription("Gets the game by specified game Id");

        group.MapGet("/", async (IGamesRepository repository, 
        ILoggerFactory loggerFactory,
        [AsParameters] GetGamesDtoV2 request,
        HttpContext http) =>
        {
            var totalCount = await repository.CountAsync(request.filter);
            http.Response.AddPaginationHeader(totalCount,request.pageSize);
            return Results.Ok((await repository.GetAllAsync(request.pageNumber,
             request.pageSize, request.filter)).Select(game => game.AsDtosV2()));
        }).MapToApiVersion(2.0)
        .WithSummary("Gets all games")
        .WithDescription("Gets all available games and allows filtering and pagination");

        group.MapGet("/{id}", async Task<Results<Ok<GameDtoV2>,NotFound>> (IGamesRepository repository, int id) =>
        {
            Game? game = await repository.GetAsync(id);
            return game is not null ? TypedResults.Ok(game.AsDtosV2()) : TypedResults.NotFound();
        })
        .WithName(GetGameV2EndpointName)
        .RequireAuthorization(Policies.Read_Access)
        .MapToApiVersion(2.0)
        .WithSummary("Get a game by Id")
        .WithDescription("Gets the game by specified game Id");;


        group.MapPost("/", async Task<CreatedAtRoute<GameDtoV1>> (IGamesRepository repository, CreateGameDto gameDto) =>
        {
            Game game = new()
            {
                Name = gameDto.Name,
                Genre = gameDto.Genre,
                Price = gameDto.Price,
                ReleaseDate = gameDto.ReleaseDate,
                ImageUri = gameDto.ImageUri
            };
            await repository.CreateAsync(game);
            return TypedResults.CreatedAtRoute(game.AsDtosV1(),GetGameV1EndpointName, new { Id = game.Id });
        }).RequireAuthorization(Policies.Write_Access)
        .MapToApiVersion(1.0)
        .WithSummary("Creates new game")
        .WithDescription("Creates new game with specified properties");;

        group.MapPut("/{id}", async Task<Results<NotFound,NoContent>> (IGamesRepository repository, int id, UpdateGameDto updatedGameDto) =>
        {
            Game? existingGame = await repository.GetAsync(id);
            if (existingGame is null)
            {
                return TypedResults.NotFound();
            }
            existingGame.Name = updatedGameDto.Name;
            existingGame.Price = updatedGameDto.Price;
            existingGame.Genre = updatedGameDto.Genre;
            existingGame.ReleaseDate = updatedGameDto.ReleaseDate;
            existingGame.ImageUri = updatedGameDto.ImageUri;

            await repository.UpdateAsync(existingGame);
            return TypedResults.NoContent();
        }).RequireAuthorization(Policies.Write_Access)
        .MapToApiVersion(1.0)
        .WithSummary("Updates a game")
        .WithDescription("Updates all games properties that has specified id");;

        group.MapDelete("/{id}", async (IGamesRepository repository, int id) =>
        {
            Game? game = await repository.GetAsync(id);
            if (game is not null)
            {
                await repository.DeleteAsync(id);
            }
            return TypedResults.NoContent();
        }).RequireAuthorization(Policies.Write_Access)
        .WithSummary("Deletes a game")
        .WithDescription("Deletes a game by specified game Id");;
        return group;

    }
}