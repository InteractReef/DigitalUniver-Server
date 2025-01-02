using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractReef.Sequrity
{
	public interface ITokenController
	{
		bool ValidateToken(string token);
		Dictionary<string, string> GetValues(string token, List<string> keys);
		string CreateToken(int userId, string email, string password);
	}
}
