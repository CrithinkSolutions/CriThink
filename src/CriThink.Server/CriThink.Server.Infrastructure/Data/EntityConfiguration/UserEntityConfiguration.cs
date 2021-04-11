using CriThink.Server.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Options;

namespace CriThink.Server.Infrastructure.Data.EntityConfiguration
{
    public class UserEntityConfiguration : IEntityTypeConfiguration<User>
    {
        private readonly IOptions<User> _serviceUser;

        public UserEntityConfiguration(IOptions<User> configuration)
        {
            //_serviceUser = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users");
            builder.Ignore(property => property.TwoFactorEnabled);
            builder.Ignore(property => property.PhoneNumberConfirmed);

            //var serviceUser = _serviceUser.Value;
            //builder.HasData(serviceUser);
        }
    }
}
