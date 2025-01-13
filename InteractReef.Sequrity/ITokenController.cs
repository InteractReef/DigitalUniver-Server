using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace InteractReef.Sequrity
{
	public interface ITokenController
	{
		string GetToken(HttpContext context);
		Dictionary<string, string> GetValues(string token, List<string> keys);
		string CreateToken(List<Claim> claims);
	}
}
