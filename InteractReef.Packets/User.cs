using InteractReef.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractReef.Packets.User
{
	public class UserModel : IEntity
	{
		public int Id { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
	}
}
