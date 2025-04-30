using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using TrackingSystemAPI.BGservices;
using TrackingSystemAPI.Data;
using TrackingSystemAPI.Models;
using TrackingSystemAPI.Models.Entities;

namespace TrackingSystemAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TagDataController : Controller
    {
        private readonly AppDbContext dbContext;
        private readonly TagDataService tagDataService;

        public TagDataController(AppDbContext dbContext, TagDataService tagDataService)
        {
            this.dbContext = dbContext;
            this.tagDataService = tagDataService;
        }


        [HttpGet]
        public async Task<IActionResult> GetTagDataAsync()
        {
            var tagData = await dbContext.TagDatas.ToListAsync();
            return Ok(tagData);
        }
       
    }
    };





