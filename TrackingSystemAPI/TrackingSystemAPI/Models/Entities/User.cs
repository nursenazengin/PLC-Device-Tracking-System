using System.ComponentModel.DataAnnotations;

namespace TrackingSystemAPI.Models.Entities
{
    public class User
    {
        [Key]
        public int user_id { get; set; }
        public string user_name { get; set; }
        public string email { get; set; }
        public string password { get; set; }

        public List<Image> Images { get; set; }

        public List<Alarm> Alarms { get; set; } 

        public List<Event> Events { get; set; }


    }
}
