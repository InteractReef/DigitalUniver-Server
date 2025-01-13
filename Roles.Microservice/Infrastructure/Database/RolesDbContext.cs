using InteractReef.Packets;
using Microsoft.EntityFrameworkCore;

namespace Roles.Microservice.Infrastructure.Database
{
	public class RolesDbContext : DbContext
	{
		public DbSet<AdminModel> Admins { get; set; }
		public DbSet<EmployeeModel> Employees { get; set; }
		public DbSet<StudentModel> Students { get; set; }

		public RolesDbContext(DbContextOptions<RolesDbContext> options)
		: base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<AdminModel>(entity =>
			{
				entity.HasKey(e => e.Id);

				entity.Property(e => e.UserId)
					  .IsRequired();

				entity.HasIndex(e => e.UserId).IsUnique();
			});

			modelBuilder.Entity<EmployeeModel>(entity =>
			{
				entity.HasKey(e => e.Id);

				entity.Property(e => e.UserId)
					  .IsRequired();

				entity.Property(e => e.OrganizationId)
					  .IsRequired();

				entity.Property(e => e.Level)
					  .IsRequired();

				entity.HasIndex(e => e.UserId).IsUnique();
			});

			modelBuilder.Entity<StudentModel>(entity =>
			{
				entity.HasKey(e => e.Id);

				entity.Property(e => e.UserId)
					  .IsRequired();

				entity.Property(e => e.GroupId)
					  .IsRequired();

				entity.Property(e => e.OrganizationId)
					  .IsRequired();

				entity.HasIndex(e => e.UserId).IsUnique();
			});
		}
	}
}
