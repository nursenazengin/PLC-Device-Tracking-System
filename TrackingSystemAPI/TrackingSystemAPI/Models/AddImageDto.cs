namespace TrackingSystemAPI.Models
{
    public class AddImageDto
    {
        public IFormFile? image_url { get; set; }

        public int user_id { get; set; }
    }
}
