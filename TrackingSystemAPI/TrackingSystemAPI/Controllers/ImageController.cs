using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrackingSystemAPI.Data;
using TrackingSystemAPI.Models;
using TrackingSystemAPI.Models.Entities;

namespace TrackingSystemAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ImageController : Controller
    {
        private readonly AppDbContext dbContext;

        public ImageController(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        [Route("{user_id:int}")]
        public IActionResult GetImage(int user_id)
        {
            var image = dbContext.Images.FirstOrDefault(i => i.user_id == user_id);

            if (image == null)
            {
                return NotFound("Kullanıcıya ait fotoğraf bulunamadı.");
            }

            return Ok(image);
        }

        [HttpPost]
        [Route("{user_id:int}")]
        public IActionResult AddImage([FromForm] AddImageDto addImageDto)
        {
            string? imagePath = null;

            if (addImageDto.image_url != null)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(addImageDto.image_url.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    addImageDto.image_url.CopyTo(stream);
                }

                imagePath = "/uploads/" + fileName;
            }


            var imageEntitiy = new Image()
            {
                user_id = addImageDto.user_id,
                image_url = imagePath
            };


            dbContext.Images.Add(imageEntitiy);
            dbContext.SaveChanges();

            return Ok(imageEntitiy);
            }

        [HttpDelete]
        [Route("{image_id:int}")]
        public IActionResult DeleteImage(int image_id)
        {

            var image = dbContext.Images.Find(image_id);

            dbContext.Images.Remove(image);
            dbContext.SaveChanges();

            return Ok(image);
        }
           

    }
}
