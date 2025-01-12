using InteractReef.Packets;
using InteractReef.Packets.Schedules;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

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
					  .HasForeignKey("ScheduleIdNumerator")
					  .OnDelete(DeleteBehavior.Cascade);

				entity.HasMany(s => s.DenominatorWeek)
					  .WithOne()
					  .HasForeignKey("ScheduleIdDenominator")
					  .OnDelete(DeleteBehavior.Cascade);
			});

			var intListComparer = new ValueComparer<List<int>>(
			(c1, c2) => c1.SequenceEqual(c2),
			c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())), 
			c => c.ToList()); 

			modelBuilder.Entity<ScheduleItem>()
				.Property(e => e.Subjects)
				.HasConversion(
					v => string.Join(',', v), 
					v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList())
				.Metadata.SetValueComparer(intListComparer);

			modelBuilder.Entity<SubjectItem>(entity =>
			{
				entity.HasKey(si => si.Id);
			});
		}
	}
}
