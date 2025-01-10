using InteractReef.Database.Core;
using InteractReef.Packets;
using InteractReef.Packets.Schedules;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Schedules.Microservice.Controllers
{
	[Route("schedules")]
	[ApiController]
	[Authorize]
	public class ScheduleController : ControllerBase
	{
		private readonly IRepository<Schedule> _schedulesRepository;

		[HttpGet]
		public IActionResult Get([FromQuery] int id)
		{
			var schedule = _schedulesRepository.GetById(id);
			return schedule != null ? Ok(schedule) : NotFound();
		}

		[HttpPost("insert")]
		public IActionResult Insert([FromBody] Schedule schedule)
		{
			_schedulesRepository.Update(schedule.Id, schedule);
			return Ok();
		}

		[HttpPost("delete")]
		public IActionResult Delete([FromQuery] int id)
		{
			var schedule = _schedulesRepository.GetById(id);
			if(schedule == null)
				return NotFound();

			_schedulesRepository.Delete(schedule);
			return Ok();
		}
	}
}
