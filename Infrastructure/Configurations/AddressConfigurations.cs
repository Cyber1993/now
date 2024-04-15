using Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class AddressConfigurations
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder
                .HasOne(c => c.User)
                .WithOne(c => c.Address)
                .HasForeignKey<Address>(c => c.UserId);
        }
    }
}
