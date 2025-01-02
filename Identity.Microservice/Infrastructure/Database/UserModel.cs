using InteractReef.Database;

namespace Identity.Microservice.Infrastructure.Database
{
	public class UserModel : IEntity
	{
		public int Id { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
	}
}
