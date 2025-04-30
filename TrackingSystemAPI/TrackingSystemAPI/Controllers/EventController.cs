using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TrackingSystemAPI.Data;
using TrackingSystemAPI.Models;
using TrackingSystemAPI.Models.Entities;

namespace TrackingSystemAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventController : Controller
    {

        private readonly AppDbContext dbContext;

        public EventController(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        [HttpPost("events")]
        public IActionResult addEvent([FromBody] AddEventDto addEventDto)
        {
            var eventEntity = new Event
            {
                user_id = addEventDto.user_id,
                EventTargets = new List<EventTarget>()
            };


            foreach (var targetDto in addEventDto.EventTargets)
            {
                var eventTargetEntity = new EventTarget
                {
                    machine_id = targetDto.machine_id,
                    machine_name = targetDto.machine_name,
                    expression = targetDto.expression,
                    logic = targetDto.logic
                };

                eventEntity.EventTargets.Add(eventTargetEntity);

            }

            dbContext.Add(eventEntity);
            dbContext.SaveChanges();

            return Ok("Event added successfully");
        }


        [HttpGet]
        [Route("{user_id:int}")]
        public IActionResult GetEventByUserId(int user_id)
        {
            var events = dbContext.Events
                .Where(u => u.user_id == user_id)
                .Select(e => new
                {
                    e.event_id,
                    e.user_id,
                    event_targets = e.EventTargets.Select(et => new
                    {
                        et.machine_name,
                        et.expression,
                        et.logic
                    })
                }
                )
                .ToList();

            if(!events.Any())
            {
                return NotFound("No events found for the given user ID.");
            }

            return Ok(events);
        }

        [HttpDelete("events/{user_id:int}/{event_id:int}")]
        public IActionResult DeleteEvents(int user_id, int event_id)
        {

            var events = dbContext.Events
                .FirstOrDefault(x => x.user_id == user_id && x.event_id == event_id);

            var relatedLogs = dbContext.EventLogs.Where(x => x.event_id == event_id);

            var relatedTargets = dbContext.EventTargets.Where(x => x.event_id == event_id);

            if (events == null)
            {
                return NotFound("Alarm not found.");
            }


            dbContext.Events.Remove(events);
            dbContext.EventLogs.RemoveRange(relatedLogs);
            dbContext.EventTargets.RemoveRange(relatedTargets);
            dbContext.SaveChanges();

            return Ok("Event deleted successfully.");
        }

    }
}
