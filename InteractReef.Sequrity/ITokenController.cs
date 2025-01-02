namespace InteractReef.Sequrity
{
	public interface ITokenController
	{
		bool ValidateToken(string token);
		Dictionary<string, string> GetValues(string token, List<string> keys);
		string CreateToken(int userId, string email, string password);
	}
}
