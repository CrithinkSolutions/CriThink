using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;

namespace CriThink.Server.Web.Models.DTOs
{
    /// <summary>
    /// Base class for successfully responses
    /// </summary>
    public class ApiOkResponse : ApiResponse
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="result">Result to return to the caller</param>
        public ApiOkResponse(object result)
            : base(StatusCodes.Status200OK)
        {
            Result = result;
        }

        /// <summary>
        /// The response result
        /// </summary>
        [JsonPropertyName("result")]
        public object Result { get; }
    }
}
