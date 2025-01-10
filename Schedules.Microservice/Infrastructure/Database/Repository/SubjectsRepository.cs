using InteractReef.Database.Core;
using InteractReef.Packets;
using InteractReef.Packets.Schedules;
using Microsoft.EntityFrameworkCore;

namespace Schedules.Microservice.Infrastructure.Database
{
	public class SubjectsRepository : GenericRepository<SchedulesDbContext, SubjectItem>
	{
		public SubjectsRepository(SchedulesDbContext dbContext)
		: base(dbContext)
		{
		}


		protected override DbSet<SubjectItem> DbSet => _dbContext.SubjectItems;
	}
}
