using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using CriThink.Server.Web.Models.DTOs;

#pragma warning disable CA1062 // Validate arguments of public methods
namespace CriThink.Server.Web.Converters
{
    public class ApiResponseConverter : JsonConverter<ApiOkResponse>
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return true;
        }

        public override ApiOkResponse Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new InvalidOperationException($"{nameof(ApiOkResponse)} is a write-only converter");
        }

        public override void Write(Utf8JsonWriter writer, ApiOkResponse value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value.Result);
        }
    }
}