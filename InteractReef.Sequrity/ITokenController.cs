namespace InteractReef.Sequrity
{
	public interface ITokenController
	{
		Dictionary<string, string> GetValues(string token, List<string> keys);
		string CreateToken(int userId, string email, string password);
	}
}
