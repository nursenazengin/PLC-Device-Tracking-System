namespace TrackingSystemAPI.Models
{
    public class AddEventDto
    {

        public int user_id { get; set; }

        public List<AddEventTargetDto> EventTargets { get; set; }
    }
}
