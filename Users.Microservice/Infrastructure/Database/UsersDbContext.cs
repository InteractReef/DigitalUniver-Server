using InteractReef.Packets.User;
using Microsoft.EntityFrameworkCore;

namespace Users.Microservice.Infrastructure.Database
{
	public class UsersDbContext : DbContext
	{
		public DbSet<UserModel> Users { get; set; }

		public UsersDbContext(DbContextOptions<UsersDbContext> options)
		: base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<UserModel>(entity =>
			{
				entity.HasKey(e => e.Id);

				entity.Property(e => e.Email)
					  .IsRequired()
					  .HasMaxLength(45);

				entity.Property(e => e.Password)
					  .IsRequired()
					  .HasMaxLength(10);

				entity.HasIndex(e => e.Email).IsUnique();
			});
		}
	}
}
