namespace TrackingSystemAPI.Models
{
    public class AddAlarmDto
    {
        public int user_id { get; set; }

        public string type { get; set; }

        public List<AddAlarmTargetDto> AlarmTargets { get; set; }
    }
}
