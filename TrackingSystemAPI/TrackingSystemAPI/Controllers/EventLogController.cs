using Microsoft.AspNetCore.Mvc;
using TrackingSystemAPI.Data;
using TrackingSystemAPI.Models.Entities;

namespace TrackingSystemAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventLogController : Controller
    {

        private readonly AppDbContext dbContext;

        public EventLogController(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        [Route("{user_id:int}")]
        public IActionResult GetEventLogsByUserId(int user_id)
        {
            var eventLogs = dbContext.EventLogs
                .Join(dbContext.Events,
                      et => et.event_id,
                      e => e.event_id,
                      (et, e) => new { et, e })
                .Join(dbContext.EventLogs,
                      combined => combined.e.event_id,
                      el => el.event_id,
                      (combined, el) => combined.e.user_id)
                .Distinct()
                .ToList();

            if(eventLogs == null)
            {
                return NotFound();
            }

            return Ok(eventLogs);
        }


        [HttpGet("events/{user_id:int}")]
        public IActionResult GetEventsByUserId(int user_id)
        {
            var eventLogs = (
                from et in dbContext.EventTargets
                join e in dbContext.Events on et.event_id equals e.event_id
                join el in dbContext.EventLogs on e.event_id equals el.event_id
                select new
                {
                    event_id = et.event_id,
                    expression = et.expression,
                    read_value = el.read_value,
                    triggered_at = el.triggered_at,
                    machine_name = et.machine_name,
                    logic = et.logic
                }
                ).ToList();

            return Ok(eventLogs);
        }

    }
}
