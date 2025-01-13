using InteractReef.Database.Core;
using InteractReef.Packets;
using Microsoft.EntityFrameworkCore;
using Roles.Microservice.Infrastructure.Database;
using System.Collections.Generic;

namespace Roles.Microservice.Infrastructure.Repository
{
	public class StudentsRepository : GenericRepository<RolesDbContext, StudentModel>
	{
		public StudentsRepository(RolesDbContext dbContext) : base(dbContext)
		{
		}

		protected override DbSet<StudentModel> DbSet => _dbContext.Students;
	}
}
