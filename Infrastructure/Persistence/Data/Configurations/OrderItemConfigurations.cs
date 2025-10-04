using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configurations
{
	internal class OrderItemConfigurations : IEntityTypeConfiguration<OrderItem>
	{
		public void Configure(EntityTypeBuilder<OrderItem> builder)
		{
			builder.ToTable("OrderItems");
			builder.Property(OI => OI.Price).HasColumnType("decimal(8,2)");

			builder.OwnsOne(OI => OI.Product);
		}
	}
}
