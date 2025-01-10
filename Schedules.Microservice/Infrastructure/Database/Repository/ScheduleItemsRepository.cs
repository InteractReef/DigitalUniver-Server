using InteractReef.Database.Core;
using InteractReef.Packets;
using Microsoft.EntityFrameworkCore;

namespace Schedules.Microservice.Infrastructure.Database
{
	public class ScheduleItemsRepository : GenericRepository<SchedulesDbContext, ScheduleItem>
	{
		public ScheduleItemsRepository(SchedulesDbContext dbContext)
		: base(dbContext)
		{
		}


		protected override DbSet<ScheduleItem> DbSet => _dbContext.ScheduleItems;
	}
}
