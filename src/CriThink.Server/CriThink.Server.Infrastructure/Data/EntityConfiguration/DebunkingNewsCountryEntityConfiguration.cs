using System;
using CriThink.Server.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CriThink.Server.Infrastructure.Data.EntityConfiguration
{
    internal class DebunkingNewsCountryEntityConfiguration : IEntityTypeConfiguration<DebunkingNewsCountry>
    {
        public void Configure(EntityTypeBuilder<DebunkingNewsCountry> builder)
        {
            builder.Ignore(dn => dn.DomainEvents);

            var countryItaly = DebunkingNewsCountry.CreateSeed(
                Guid.Parse("575003e3-991c-4ac5-9ce4-3399553f64a7"),
                "Italy",
                "it");

            var countryUsa = DebunkingNewsCountry.CreateSeed(
                Guid.Parse("3bd76fc5-5463-4194-b4f9-df111f7c294f"),
                "United States of America",
                "us");

            var countryUk = DebunkingNewsCountry.CreateSeed(
                Guid.Parse("812361b1-d1c3-4315-b601-4e060364a1d6"),
                "United Kingdom",
                "uk");

            builder.HasKey(dnc => dnc.Id);
            builder.Property(dnc => dnc.Id)
                .ValueGeneratedOnAdd();

            builder.Property(dnc => dnc.Name)
                .IsRequired();

            builder.Property(dnc => dnc.Code)
                .IsRequired();

            builder.HasIndex(dnc => dnc.Code)
                .IsUnique();

            builder.HasData(countryItaly, countryUsa, countryUk);
        }
    }
}
