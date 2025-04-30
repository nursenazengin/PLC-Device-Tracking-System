using System.ComponentModel.DataAnnotations;

namespace TrackingSystemAPI.Models.Entities
{
    public class Image
    {
        [Key]
        public int image_id { get; set; }
        public int user_id { get; set; }

        public string image_url { get; set; }

        public User User { get; set; }
    }
}
