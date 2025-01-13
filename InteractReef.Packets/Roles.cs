using InteractReef.Database;

namespace InteractReef.Packets
{
	public class AdminModel : IEntity
	{
		public int Id { get; set; }
		public int UserId { get; set; }
	}

	public class EmployeeModel : IEntity
	{
		public int Id { get; set; }
		public int UserId { get; set; }
		public int OrganizationId { get; set; }
		public int Level { get; set; }
	}

	public class StudentModel : IEntity
	{
		public int Id { get; set; }
		public int UserId { get; set; }
		public int OrganizationId { get; set; }
		public int GroupId { get; set; }
	}
}
