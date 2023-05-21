using GameStore.API.Authorization;
using GameStore.API.Dtos;
using GameStore.API.ImageUpload;
using Microsoft.AspNetCore.Http.HttpResults;

namespace GameStore.API.Endpoints;

public static class ImagesEndpoints
{
    public static RouteHandlerBuilder MapImagesEndpoints(this IEndpointRouteBuilder routes)
    {
        var api = routes.NewVersionedApi();

        return api.MapPost(
            "/images",
            async Task<Results<Ok<ImageUploadDto>,BadRequest>>(IFormFile file, IImageUploader imageUploader) => {
                if(file.Length <= 0) return TypedResults.BadRequest();
                var imageUri = await imageUploader.UploadImageAsync(file);
                return TypedResults.Ok(new ImageUploadDto(imageUri));
        })
        .RequireAuthorization(Policies.Write_Access)
        .HasApiVersion(1.0)
        .WithOpenApi()
        .WithSummary("Uploads a file to storage")
        .WithDescription("Uploads a file to storage and returns the image url of the uploaded file.")
        .WithTags("Images");
    }
}