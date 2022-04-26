using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;

namespace CriThink.Client.Core.Exceptions
{
    internal class CriThinkSignUpException : Exception
    {
        private readonly string _content;
        private string _errorList;

        public CriThinkSignUpException(
            string content)
        {
            _content = content;
        }

        public string GetErrorList()
        {
            if (!string.IsNullOrWhiteSpace(_errorList))
                return _errorList;

            var content = System.Text.Json.JsonSerializer.Deserialize<ApiBadRequestResponse>(_content);

            var keyError = content.Errors.FirstOrDefault(); // Assume one error type
            
            var sb = new StringBuilder(keyError.Value.Count);

            foreach (var error in keyError.Value)
            {
                sb.AppendLine(error);
            }

            _errorList = sb.ToString();

            return _errorList;
        }
    }

#pragma warning disable CA2227 // Collection properties should be read only
    public class ApiBadRequestResponse
    {
        [JsonPropertyName("errors")]
        public IDictionary<string, List<string>> Errors { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; }
    }
#pragma warning restore CA2227 // Collection properties should be read only
}
