using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CriThink.Server.Infrastructure.SocialProviders
{
    internal class GoogleGetMeResponse
    {
        [JsonPropertyName("genders")]
        public List<GoogleGender> Genders { get; set; }

        [JsonPropertyName("birthdays")]
        public List<GoogleBirthday> Birthdays { get; set; }
    }

    public class GoogleBirthday
    {
        [JsonPropertyName("date")]
        public GoogleDate Date { get; set; }
    }

    public class GoogleDate
    {
        [JsonPropertyName("year")]
        public int Year { get; set; }

        [JsonPropertyName("month")]
        public int Month { get; set; }

        [JsonPropertyName("day")]
        public int Day { get; set; }
    }

    public class GoogleGender
    {
        [JsonPropertyName("value")]
        public string Value { get; set; }

        [JsonPropertyName("formattedValue")]
        public string FormattedValue { get; set; }
    }
}
