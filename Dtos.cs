using System.ComponentModel.DataAnnotations;

namespace GameStore.API.Dtos;
public record GetGamesDtoV1(
    int pageNumber = 1,
    int pageSize = 5,
    string? filter = null
);

public record GameDtoV1(
    int Id,
    string Name,
    string Genre,
    decimal Price,
    DateTime ReleaseDate,
    string ImageUri
);

public record GetGamesDtoV2(
    int pageNumber = 1,
    int pageSize = 5,
    string? filter = null
);
public record GameDtoV2(
    int Id,
    string Name,
    string Genre,
    decimal Price,
    decimal RetailPrice,
    DateTime ReleaseDate,
    string ImageUri
);
public record CreateGameDto(
    [Required][StringLength(50)] string Name,
    [Required][StringLength(20)] string Genre,
    [Range(1, 99)] decimal Price,
    DateTime ReleaseDate,
    [Url][StringLength(100)] string ImageUri
);
public record UpdateGameDto(
    [Required][StringLength(50)] string Name,
    [Required][StringLength(20)] string Genre,
    [Range(1, 99)] decimal Price,
    DateTime ReleaseDate,
    [Url][StringLength(100)] string ImageUri
);

public record ImageUploadDto(string BlobUri);
