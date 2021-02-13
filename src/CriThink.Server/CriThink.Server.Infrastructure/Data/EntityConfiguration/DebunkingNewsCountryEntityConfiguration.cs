using System;
using System.Collections.Generic;
using CriThink.Server.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CriThink.Server.Infrastructure.Data.EntityConfiguration
{
    internal class DebunkingNewsCountryEntityConfiguration : IEntityTypeConfiguration<DebunkingNewsCountry>
    {
        public void Configure(EntityTypeBuilder<DebunkingNewsCountry> builder)
        {
            var countryItaly = new DebunkingNewsCountry
            {
                Id = Guid.Parse("575003e3-991c-4ac5-9ce4-3399553f64a7"),
                Name = "Italy",
                Code = "it",
            };
            var countryUsa = new DebunkingNewsCountry
            {
                Id = Guid.Parse("3bd76fc5-5463-4194-b4f9-df111f7c294f"),
                Name = "United States of America",
                Code = "us",
            };
            var countryUk = new DebunkingNewsCountry
            {
                Id = Guid.Parse("812361b1-d1c3-4315-b601-4e060364a1d6"),
                Name = "United Kingdom",
                Code = "uk",
            };

            builder.HasIndex(c => c.Code).IsUnique();
            builder.HasData((IEnumerable<DebunkingNewsCountry>) new[] { countryItaly, countryUsa, countryUk });
        }
    }
}
