using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrackingSystemAPI.Data;

namespace TrackingSystemAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AlarmLogController : ControllerBase
    {

        private readonly AppDbContext dbContext;


        public AlarmLogController(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        [HttpGet]
        [Route("{user_id:int}")]
        public IActionResult GetAlarmLogsByUserId(int user_id)
        {
            var alarmLogs =  dbContext.AlarmLogs
                .Join(dbContext.Alarms,
                      at => at.alarm_id,
                      a => a.alarm_id,
                      (at, a) => new { at, a })
                .Join(dbContext.AlarmLogs,
                      combined => combined.a.alarm_id,
                      al => al.alarm_id,
                      (combined, al) => combined.a.user_id)
                .Distinct()
                .ToList();

            if (alarmLogs == null)
            {
                return NotFound();
            }

            return Ok(alarmLogs);
        }


        [HttpGet("alarms/{user_id:int}/{type}")]
        public IActionResult GetAlarmsByUserId(int user_id, string type)
        {

            var alarmLogs = (

                from at in dbContext.AlarmTargets
                join a in dbContext.Alarms on at.alarm_id equals a.alarm_id
                join al in dbContext.AlarmLogs on a.alarm_id equals al.alarm_id
                select new
                {
                    alarm_id = at.alarm_id,
                    expression = at.expression,
                    read_value = al.read_value,
                    triggered_at = al.triggered_at,
                    machine_name = at.machine_name,
                    type = a.type,
                    logic = at.logic
                }
            ).ToList();

            return Ok(alarmLogs);
        }




    }
}
