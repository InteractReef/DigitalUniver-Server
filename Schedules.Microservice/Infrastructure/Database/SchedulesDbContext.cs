using InteractReef.Packets;
using InteractReef.Packets.Schedules;
using Microsoft.EntityFrameworkCore;

namespace Schedules.Microservice.Infrastructure.Database
{
	public class SchedulesDbContext : DbContext
	{
		public DbSet<Schedule> Schedules { get; set; }
		public DbSet<ScheduleItem> ScheduleItems { get; set; }
		public DbSet<SubjectItem> SubjectItems { get; set; }

		public SchedulesDbContext(DbContextOptions<SchedulesDbContext> options)
		: base(options)
		{

		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Schedule>(entity =>
			{
				entity.HasKey(s => s.Id);
				entity.HasMany(s => s.NumeratorWeek)
					  .WithOne()
					  .HasForeignKey("ScheduleId")
					  .OnDelete(DeleteBehavior.Cascade);

				entity.HasMany(s => s.DenominatorWeek)
					  .WithOne()
					  .HasForeignKey("ScheduleId")
					  .OnDelete(DeleteBehavior.Cascade);
			});

			modelBuilder.Entity<ScheduleItem>(entity =>
			{
				entity.HasKey(si => si.Id);
				entity.Property(si => si.Subjects)
					  .HasConversion(
						  v => string.Join(",", v),
						  v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList()
					  );
			});

			modelBuilder.Entity<SubjectItem>(entity =>
			{
				entity.HasKey(si => si.Id);
			});
		}
	}
}
