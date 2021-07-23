using System.Text.Json;
using API.Helpers;
using Microsoft.AspNetCore.Http;

namespace API.Extensions
{
    public static class HttpExtensions
    {
        /// <summary>
        /// Attaches pagination info as a HTTP header to response. 
        /// </summary>
        /// <param name="response">Outgoing side of an individual HTTP request.</param>
        /// <param name="currentPage">Current page number</param>
        /// <param name="itemsPerPage">Number of items on single page.</param>
        /// <param name="totalItems">Number of  all items to retrieve.</param>
        /// <param name="totalPages">Count of all pages</param>
        public static void AddPaginationHeader(this HttpResponse response, int currentPage,
            int itemsPerPage, int totalItems, int totalPages)
        {
            var paginationHeader = new PaginationHeader(currentPage, itemsPerPage, totalItems, totalPages);
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            response.Headers.Add("Pagination", JsonSerializer.Serialize(paginationHeader, options));
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
        }
    }
}