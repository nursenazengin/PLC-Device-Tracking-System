 using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using TrackingSystemAPI.Data;
using TrackingSystemAPI.Models;
using TrackingSystemAPI.Models.Entities;

namespace TrackingSystemAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class MachineTagController : Controller
    {
        private readonly AppDbContext dbContext;

        public MachineTagController(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult GetMachineTagsById(int machine_id)
        {
            var machineTags = dbContext.MachineTags
                .Where(mt => mt.machine_id == machine_id)
                .Select(mt => new
                {
                    mt.tag_id,
                    tag_name = mt.Tag.tag_name,
                    tag_data_type = mt.Tag.tag_data_type   
                })
                .ToList();
            return Ok(machineTags);
        }


        [HttpPost("machinetags")]
        public IActionResult AddMachineTag(AddMachineTagDto addMachineTagDto)
        {

            var machineTag = new MachineTag
            {
                machine_id = addMachineTagDto.machine_id,
                tag_id = addMachineTagDto.tag_id,
                machine_name =addMachineTagDto.machine_name,
                tag_name = addMachineTagDto.tag_name
            };

            dbContext.Add(machineTag);
            dbContext.SaveChanges();
            return Ok(machineTag);
        }
    }
}
