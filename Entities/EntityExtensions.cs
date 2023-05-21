using GameStore.API.Dtos;

namespace GameStore.API.Entities;

public static class EntityExtensions
{
    public static GameDtoV1 AsDtosV1(this Game game)
    {
        return new GameDtoV1(
            game.Id,
            game.Name,
            game.Genre,
            game.Price,
            game.ReleaseDate,
            game.ImageUri);
    }
    public static GameDtoV2 AsDtosV2(this Game game)
    {
        return new GameDtoV2(
            game.Id,
            game.Name,
            game.Genre,
            game.Price - (game.Price * .3m),
            game.Price,
            game.ReleaseDate,
            game.ImageUri);
    }
}