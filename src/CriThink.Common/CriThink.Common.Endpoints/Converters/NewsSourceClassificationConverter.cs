using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using CriThink.Common.Endpoints.DTOs.NewsSource;

namespace CriThink.Common.Endpoints.Converters
{
    public class NewsSourceClassificationConverter : JsonConverter<NewsSourceClassification>
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert == typeof(NewsSourceClassification);
        }

        public override NewsSourceClassification Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.String) throw new Exception();

            var value = reader.GetString();

            switch (value)
            {
                case "Conspiracist":
                    return NewsSourceClassification.Conspiracist;
                case "Fake News":
                    return NewsSourceClassification.FakeNews;
                case "Reliable":
                    return NewsSourceClassification.Reliable;
                case "Satirical":
                    return NewsSourceClassification.Satirical;
                default:
                    throw new Exception();
            }
        }

        public override void Write(Utf8JsonWriter writer, NewsSourceClassification value, JsonSerializerOptions options)
        {
            if (value == NewsSourceClassification.FakeNews) writer.WriteStringValue("Fake News");

            else writer.WriteStringValue(value.ToString());
        }
    }
}