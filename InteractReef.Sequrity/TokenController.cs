using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace InteractReef.Sequrity
{
	public class TokenController : ITokenController
	{
		private readonly string _secret;

		public TokenController(IConfiguration configuration)
		{
			_secret = configuration["Sequrity:JWT_Secret"];
		}

		public Dictionary<string, string> GetValues(string token, List<string> keys)
		{
			try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadToken(token) as JwtSecurityToken;

                if (jwtToken == null)
                {
                    return null;
                }

                var claims = jwtToken.Claims.ToDictionary(c => c.Type, c => c.Value);

                var result = new Dictionary<string, string>();
                foreach (var key in keys)
                {
                    if (claims.TryGetValue(key, out var value))
                    {
                        result.Add(key, value);
                    }
                }
                return result;
            }
            catch
            {
                return null;
            }
		}

		public string CreateToken(int userId, string email, string password)
		{
			var claims = new List<Claim> {
				new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
				new Claim(ClaimTypes.Email, email),
				new Claim(ClaimTypes.UserData, password),
			};

			var jwtToken = new JwtSecurityToken(
				claims: claims,
				notBefore: DateTime.UtcNow,
				expires: DateTime.UtcNow.AddDays(30),
				signingCredentials: new SigningCredentials(
					new SymmetricSecurityKey(
					   Encoding.UTF8.GetBytes(_secret)
						),
					SecurityAlgorithms.HmacSha256Signature)
				);
			return new JwtSecurityTokenHandler().WriteToken(jwtToken);
		}
	}
}
