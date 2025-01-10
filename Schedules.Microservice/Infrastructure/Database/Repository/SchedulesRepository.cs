using InteractReef.Database.Core;
using InteractReef.Packets;
using InteractReef.Packets.Schedules;
using Microsoft.EntityFrameworkCore;

namespace Schedules.Microservice.Infrastructure.Database.Repository
{
	public class SchedulesRepository : IRepository<Schedule>
	{
		private readonly SchedulesDbContext _dbContext;
		private readonly DbSet<Schedule> _schedules;

		public SchedulesRepository(SchedulesDbContext dbContext)
		{
			_dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
			_schedules = _dbContext.Schedules;
		}

		public Schedule Add(Schedule entity)
		{
			_schedules.Add(entity);
			_dbContext.SaveChanges();
			return entity;
		}

		public Schedule Update(int id, Schedule entity)
		{
			var existingSchedule = _schedules
				.Include(s => s.NumeratorWeek)
				.Include(s => s.DenominatorWeek)
				.FirstOrDefault(s => s.Id == id);

			if (existingSchedule == null)
				throw new InvalidOperationException($"Schedule with ID {id} not found.");

			_dbContext.Entry(existingSchedule).CurrentValues.SetValues(entity);

			UpdateScheduleItems(existingSchedule.NumeratorWeek, entity.NumeratorWeek);
			UpdateScheduleItems(existingSchedule.DenominatorWeek, entity.DenominatorWeek);

			_dbContext.SaveChanges();
			return existingSchedule;
		}

		public void Delete(Schedule entity)
		{
			_dbContext.RemoveRange(entity.NumeratorWeek);
			_dbContext.RemoveRange(entity.DenominatorWeek);

			_schedules.Remove(entity);
			_dbContext.SaveChanges();
		}

		public virtual IQueryable<Schedule> GetAll()
		{
			return _schedules
				.Include(s => s.NumeratorWeek)
				.Include(s => s.DenominatorWeek);
		}

		public virtual Schedule? GetById(int id)
		{
			return _schedules
				.Include(s => s.NumeratorWeek)
				.Include(s => s.DenominatorWeek)
				.FirstOrDefault(s => s.Id == id);
		}

		private void UpdateScheduleItems(List<ScheduleItem> existingItems, List<ScheduleItem> newItems)
		{
			_dbContext.RemoveRange(existingItems.Except(newItems));

			foreach (var newItem in newItems.Except(existingItems))
			{
				_dbContext.Entry(newItem).State = EntityState.Added;
			}

			foreach (var existingItem in existingItems)
			{
				var updatedItem = newItems.FirstOrDefault(i => i.Id == existingItem.Id);
				if (updatedItem != null)
				{
					_dbContext.Entry(existingItem).CurrentValues.SetValues(updatedItem);
				}
			}
		}
	}
}
