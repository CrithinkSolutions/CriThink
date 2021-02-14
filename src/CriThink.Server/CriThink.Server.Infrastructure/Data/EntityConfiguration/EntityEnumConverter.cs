using System;

namespace CriThink.Server.Infrastructure.Data.EntityConfiguration
{
    internal static class EntityEnumConverter
    {
        public static TEnum GetEnumValue<TEnum>(string value)
            where TEnum : Enum
        {
            return (TEnum) Enum.Parse(typeof(TEnum), value);
        }
    }
}
