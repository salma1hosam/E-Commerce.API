using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configurations
{
	internal class OrderConfigurations : IEntityTypeConfiguration<Order>
	{
		public void Configure(EntityTypeBuilder<Order> builder)
		{
			builder.ToTable("Orders");
			builder.Property(O => O.SubTotal).HasColumnType("decimal(8,2)");

			builder.HasMany(O => O.Items)
				   .WithOne()
				   .OnDelete(DeleteBehavior.Cascade); //Will Create the FK in the DB By Default.

			builder.HasOne(O => O.DeliveryMethod)
				   .WithMany()
				   .HasForeignKey(O => O.DeliveryMethodId);

			builder.OwnsOne(O => O.ShipToAddress);
		}
	}
}
