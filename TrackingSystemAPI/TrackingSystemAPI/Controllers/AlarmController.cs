using Microsoft.AspNetCore.Mvc;
using TrackingSystemAPI.Data;
using TrackingSystemAPI.Models;
using TrackingSystemAPI.Models.Entities;

namespace TrackingSystemAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AlarmController : Controller
    {

        private readonly AppDbContext dbContext;

        public AlarmController(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        [HttpPost("alarms")]
        public IActionResult AddAlarm([FromBody] AddAlarmDto addAlarmDto)
        {
            var alarmEntity = new Alarm
            {
                user_id = addAlarmDto.user_id,
                type = addAlarmDto.type,
                AlarmTargets = new List<AlarmTarget>()
            };

            foreach (var targetDto in addAlarmDto.AlarmTargets)
            {
                var alarmTargetEntity = new AlarmTarget
                {
                    machine_id = targetDto.machine_id,
                    machine_name = targetDto.machine_name,
                    expression = targetDto.expression,
                    logic = targetDto.logic
                };

                alarmEntity.AlarmTargets.Add(alarmTargetEntity);
            }

            dbContext.Add(alarmEntity);
            dbContext.SaveChanges();

            return Ok("Alarm added successfully");
        }

        [HttpGet("alarms/{user_id:int}/{type}")]
        public IActionResult GetAlarmsByUserId(int user_id, string type)
        {

            var alarms = dbContext.Alarms
                .Where(x => x.user_id == user_id && x.type == type)
                .Select(a => new
                {
                    a.alarm_id,
                    a.user_id,
                    a.type,
                    alarm_targets = a.AlarmTargets.Select(at => new
                    {
                        at.machine_name,
                        at.alarm_id,
                        at.expression,
                        at.logic

                    })
                })
                .ToList();



            if (!alarms.Any())
            {
                return NotFound("No alarms found for the given user_id and type.");
            }

            return Ok(alarms);
        }


        [HttpDelete("alarms/{user_id:int}/{alarm_id:int}")]
        public IActionResult DeleteAlarms(int user_id, int alarm_id)
        {

            var alarm = dbContext.Alarms
                .FirstOrDefault(x => x.user_id == user_id && x.alarm_id == alarm_id);

            var relatedLogs = dbContext.AlarmLogs.Where(x => x.alarm_id == alarm_id);

            var relatedTargets = dbContext.AlarmTargets.Where(x => x.alarm_id == alarm_id);

            if (alarm == null)
            {
                return NotFound("Alarm not found.");
            }


            dbContext.Alarms.Remove(alarm);
            dbContext.AlarmLogs.RemoveRange(relatedLogs);
            dbContext.AlarmTargets.RemoveRange(relatedTargets);
            dbContext.SaveChanges();

            return Ok("Alarm deleted successfully.");
        }




    }
}
