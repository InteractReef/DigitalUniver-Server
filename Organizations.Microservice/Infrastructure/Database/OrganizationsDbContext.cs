using InteractReef.Packets;
using InteractReef.Packets.Organizations;
using Microsoft.EntityFrameworkCore;

namespace Organizations.Microservice.Infrastructure.Database
{
	public class OrganizationsDbContext : DbContext
	{
		public DbSet<OrganizationModel> Organizations { get; set; }

		public OrganizationsDbContext(DbContextOptions<OrganizationsDbContext> options)
		: base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<OrganizationModel>(entity =>
			{
				entity.HasKey(e => e.Id);

				entity.Property(e => e.FullName)
					  .IsRequired()
					  .HasMaxLength(60);

				entity.Property(e => e.ShortName)
					  .IsRequired()
					  .HasMaxLength(8);

				entity.HasMany(s => s.Groups)
					.WithOne()
					.HasForeignKey("GroupId")
					.OnDelete(DeleteBehavior.Cascade);
			});
		}
	}
}
