using InteractReef.Database;

namespace InteractReef.Packets.Schedules
{
	public class Schedule : IEntity
	{
		public int Id { get; set; }
		public int OrgId { get; set; }

		public required List<ScheduleItem> NumeratorWeek { get; set; }
		public required List<ScheduleItem> DenominatorWeek { get; set; }
	}

	public class  ScheduleItem : IEntity
	{
		public int Id { get; set; }
		public required List<int> Subjects { get; set; }
	}

	public class SubjectItem : IEntity
	{
		public int Id { get; set; }
		public required string Name { get; set; }
	}
}
