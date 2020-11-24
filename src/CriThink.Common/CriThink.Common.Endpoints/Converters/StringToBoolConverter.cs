using System;
using System.Text.Json;
using System.Text.Json.Serialization;

#pragma warning disable CA1062 // Validate arguments of public methods
namespace CriThink.Common.Endpoints.Converters
{
    public class StringToBoolConverter : JsonConverter<bool>
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert == typeof(bool);
        }

        public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                var stringValue = reader.GetString();

                return bool.Parse(stringValue);
            }

            return false;
        }

        public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
