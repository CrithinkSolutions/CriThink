using System.Text.Json.Serialization;

namespace CriThink.Server.Web.Models.DTOs
{
    /// <summary>
    /// Basic class for response DTO
    /// </summary>
    public class ApiResponse
    {
        /// <summary>
        /// Construct the basic response
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="message"></param>
        public ApiResponse(int statusCode, string message = null)
        {
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }

        /// <summary>
        /// Get the response message
        /// </summary>
        [JsonPropertyName("message")]
        public string Message { get; }

        private static string GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "The request is invalid",
                404 => "Resource not found",
                500 => "An unhandled error occurred",
                _ => null
            };
        }
    }
}
