using System.Text.Json.Serialization;
using CriThink.Server.Web.Converters;
using Microsoft.AspNetCore.Http;

namespace CriThink.Server.Web.Models.DTOs
{
    /// <summary>
    /// Base class for successfully responses
    /// </summary>
    [JsonConverter(typeof(ApiResponseConverter))]
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
        [JsonIgnore]
        public object Result { get; }
    }
}