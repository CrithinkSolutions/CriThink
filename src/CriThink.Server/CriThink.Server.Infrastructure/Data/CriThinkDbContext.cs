﻿using System;
using System.Diagnostics.CodeAnalysis;
using CriThink.Server.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CriThink.Server.Infrastructure.Data
{
    public class CriThinkDbContext : IdentityDbContext<User, UserRole, Guid>
    {
        public CriThinkDbContext(DbContextOptions<CriThinkDbContext> context)
            : base(context)
        { }

        [SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "Injected")]
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>(typeBuilder =>
            {
                typeBuilder.ToTable("Users");
                typeBuilder.Ignore(property => property.ConcurrencyStamp);
                typeBuilder.Ignore(property => property.SecurityStamp);
                typeBuilder.Ignore(property => property.ConcurrencyStamp);
                typeBuilder.Ignore(property => property.TwoFactorEnabled);
                typeBuilder.Ignore(property => property.PhoneNumberConfirmed);
            });

            builder.Entity<UserRole>(typeBuilder =>
            {
                typeBuilder.ToTable("UserRoles");
                typeBuilder.Ignore(property => property.NormalizedName);
                typeBuilder.Ignore(property => property.ConcurrencyStamp);
            });
        }
    }
}
