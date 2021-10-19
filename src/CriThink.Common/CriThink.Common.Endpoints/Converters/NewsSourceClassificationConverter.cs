using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using CriThink.Common.Endpoints.DTOs.NewsSource;

#pragma warning disable CA1062 // Validate arguments of public methods
namespace CriThink.Common.Endpoints.Converters
{
    public class NewsSourceClassificationConverter : JsonConverter<NewsSourceAuthenticityDto>
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert == typeof(NewsSourceAuthenticityDto);
        }

        public override NewsSourceAuthenticityDto Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.String) throw new Exception();

            var value = reader.GetString();

            return value switch
            {
                "Conspiracist" => NewsSourceAuthenticityDto.Conspiracist,
                "Fake News" => NewsSourceAuthenticityDto.FakeNews,
                "Reliable" => NewsSourceAuthenticityDto.Reliable,
                "Satirical" => NewsSourceAuthenticityDto.Satirical,
                "Suspicious" => NewsSourceAuthenticityDto.Suspicious,
                "Social Media" => NewsSourceAuthenticityDto.SocialMedia,
                "Unknown" => NewsSourceAuthenticityDto.Unknown,
                _ => throw new Exception(),
            };
        }

        public override void Write(Utf8JsonWriter writer, NewsSourceAuthenticityDto value, JsonSerializerOptions options)
        {
            switch (value)
            {
                case NewsSourceAuthenticityDto.FakeNews:
                    writer.WriteStringValue("Fake News");
                    break;
                case NewsSourceAuthenticityDto.SocialMedia:
                    writer.WriteStringValue("Social Media");
                    break;
                default:
                    writer.WriteStringValue(value.ToString());
                    break;
            }
        }
    }
}