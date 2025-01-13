using InteractReef.Database.Core;
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

		public ScheduleController(IRepository<Schedule> schedulesRepository) 
		{
			_schedulesRepository = schedulesRepository;
		}

		[HttpGet]
		public IActionResult Get([FromQuery] int id)
		{
			var schedule = _schedulesRepository.GetById(id);
			return schedule != null ? Ok(schedule) : NotFound();
		}

		[HttpPost("add")]
		public IActionResult Add([FromBody] Schedule schedule)
		{
			_schedulesRepository.Add(schedule);
			return Ok();
		}

		[HttpPost("update")]
		public IActionResult Update([FromBody] Schedule schedule)
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
