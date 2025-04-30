using Microsoft.AspNetCore.Mvc;
using TrackingSystemAPI.Models;
using TrackingSystemAPI.Models.Entities;
using TrackingSystemAPI.Data;
using System;

namespace TrackingSystemAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class TagController : ControllerBase
    {

        private readonly AppDbContext dbContext;

        public TagController(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        [HttpPost("tags")]
        public IActionResult AddTag(AddTagDto addTagDto)
        {

            var tagEntity = new Tag
            {

                tag_name = addTagDto.tag_name,
                tag_data_type = addTagDto.tag_data_type
            };

            dbContext.Tags.Add(tagEntity);
            dbContext.SaveChanges();

            return Ok(tagEntity);
        }
    }
}
