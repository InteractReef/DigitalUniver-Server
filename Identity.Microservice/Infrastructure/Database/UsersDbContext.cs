using Microsoft.EntityFrameworkCore;

namespace Identity.Microservice.Infrastructure.Database
{
	public class UsersDbContext : DbContext
	{
		public UsersDbContext(DbContextOptions<UsersDbContext> options)
		: base(options)
		{
		}

		public virtual DbSet<UserModel> Users { get; set; }

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
