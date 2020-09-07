using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;

#pragma warning disable CA1062 // Validate arguments of public methods

namespace CriThink.Server.Web.Models.DTOs
{
    /// <summary>
    /// Base class for bad responses
    /// </summary>
    public class ApiBadRequestResponse : ApiResponse
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="modelState">State of bind values</param>
        public ApiBadRequestResponse(ModelStateDictionary modelState)
            : base(StatusCodes.Status400BadRequest)
        {
            if (modelState.IsValid)
            {
                throw new ArgumentException("ModelState must be invalid", nameof(modelState));
            }

            Errors = modelState.ToDictionary(
                state => string.IsNullOrWhiteSpace(state.Key) ? "body" : state.Key,
                entry =>
                {
                    return entry.Value.Errors
                        .Select(e => e.ErrorMessage)
                        .ToList()
                        .AsReadOnly();
                });
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="resource">Name of the resource that failed the validation</param>
        /// <param name="messages">List of messages to display</param>
        public ApiBadRequestResponse(string resource, IEnumerable<string> messages)
            : base(StatusCodes.Status400BadRequest)
        {
            Errors = new Dictionary<string, ReadOnlyCollection<string>>
            {
                {resource, messages.ToList().AsReadOnly()}
            };
        }

        /// <summary>
        /// Get the list of errors
        /// </summary>
        [JsonPropertyName("errors")]
        public IDictionary<string, ReadOnlyCollection<string>> Errors { get; }
    }
}
