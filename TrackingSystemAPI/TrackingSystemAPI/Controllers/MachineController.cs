using Microsoft.AspNetCore.Mvc;
using System;
using TrackingSystemAPI.Data;
using TrackingSystemAPI.Models;
using TrackingSystemAPI.Models.Entities;

namespace TrackingSystemAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class MachineController : Controller
    {
        private readonly AppDbContext dbContext;

        public MachineController(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult GetMachines()
        {
            var machines = dbContext.Machines.ToList();
            return Ok(machines);
        }


        [HttpPost("machines")]
        public IActionResult AddMachine(AddMachineDto addMachineDto)
        {

            var machineEntity = new Machine
            {
                machine_name = addMachineDto.machine_name,

            };

            dbContext.Machines.Add(machineEntity);
            dbContext.SaveChanges();

            return Ok(machineEntity);
        }
    }
}
