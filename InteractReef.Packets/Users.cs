using InteractReef.Database;

namespace InteractReef.Packets.Users
{
	public class UserModel : IEntity
	{
		public int Id { get; set; }
		public required string Email { get; set; }
		public required string Password { get; set; }
	}

	public record UserOperation(UserOperationType type, IUserOperationParam param);

	public enum UserOperationType
	{
		AddOrUpdate, Delete
	}

	public interface IUserOperationParam {}

	public class DeleteUserOperation : IUserOperationParam
	{
		public int UserId { get; set; }
	}

	public class AddUserOperation : IUserOperationParam
	{
		public required UserModel Model { get; set; }
	}
}
