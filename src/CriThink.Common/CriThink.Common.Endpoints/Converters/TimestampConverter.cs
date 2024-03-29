﻿using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using CriThink.Common.Helpers;

#pragma warning disable CA1305 // Specify IFormatProvider
#pragma warning disable CA1062 // Validate arguments of public methods
namespace CriThink.Common.Endpoints.Converters
{
    public class TimestampConverter : JsonConverter<DateTime>
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert == typeof(DateTime);
        }

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                var stringValue = reader.GetString();

                var timestamp = long.Parse(stringValue, CultureInfo.InvariantCulture);

                var dateTime = timestamp.AsUnixTimestamp();

                return dateTime;
            }

            throw new NotSupportedException();
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            var timestamp = value.ToUnixTimestamp();

            writer.WriteStringValue(timestamp.ToString());
        }
    }
}