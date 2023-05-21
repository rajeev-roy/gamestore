using System.Text.Json;

namespace GameStore.API.Endpoints;

public static class HttpResponseExtensions
{
    public static void AddPaginationHeader(this HttpResponse response,
    int totalCount, int pageSize)
    {
        var PaginationHeader = new {
            totalPages = (int)Math.Ceiling(totalCount/(double)pageSize)

        };
        response.Headers.Add("X-Pagination", JsonSerializer.Serialize(PaginationHeader));
    }
}