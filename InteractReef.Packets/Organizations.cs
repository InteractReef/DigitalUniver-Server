using InteractReef.Database;

namespace InteractReef.Packets.Organizations
{
	public class OrganizationModel : IEntity
	{
		public int Id { get; set; }
		public required string FullName { get; set; }
		public required string ShortName { get; set; }
		public required List<GroupModel> Groups { get; set; }
	}

	public class GroupModel : IEntity
	{
		public int Id { get; set; }
		public required string Name { get; set; }
	}
}
