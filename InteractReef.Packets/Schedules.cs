using InteractReef.Database;

namespace InteractReef.Packets
{
	public class Schedule : IEntity
	{
		public int Id { get; }
		public int OrgId { get; }

		public List<ScheduleItem> numeratorWeek { get; }
		public List<ScheduleItem> denominatorWeek { get; }
	}

	public class  ScheduleItem : IEntity
	{
		public int Id { get; }
		public List<int> Subjects { get; }
	}

	public class SubjectItem : IEntity
	{
		public int Id { get; }
		public string Name { get; }
	}
}
