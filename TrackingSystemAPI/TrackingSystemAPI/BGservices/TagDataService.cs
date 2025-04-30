using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using TrackingSystemAPI.Data;
using TrackingSystemAPI.Models.Entities;

namespace TrackingSystemAPI.BGservices
{
    public class TagDataService
    {
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly EvaluateAlarmService evaluateAlarmService;
        private readonly EvaluateEventService evaluateEventService;

        public TagDataService(IServiceScopeFactory serviceScopeFactory, 
                              EvaluateAlarmService evaluateAlarmService,
                              EvaluateEventService evaluateEventService)
        {
            this.serviceScopeFactory = serviceScopeFactory;
            this.evaluateAlarmService = evaluateAlarmService;
            this.evaluateEventService = evaluateEventService;
        }

        public async Task AddTagDataAsync()
        {
            Random random = new Random();
            while (true)
            {
                using (var scope = serviceScopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    var alarmService = scope.ServiceProvider.GetRequiredService<EvaluateAlarmService>();
                    var eventService = scope.ServiceProvider.GetRequiredService<EvaluateEventService>();

                    var machineTags = await dbContext.MachineTags.ToListAsync();
                    var tagValues = GenerateTagValues(machineTags);

                    var generatedTagDataList = new List<TagData>();

                    foreach (var machineTag in machineTags)
                    {
                        var value = tagValues[(machineTag.machine_id, machineTag.tag_name)];

                        var tagData = new TagData
                        {
                            tag_id = machineTag.tag_id,
                            machine_id = machineTag.machine_id,
                            tag_name = machineTag.tag_name,
                            machine_name = machineTag.machine_name,
                            recorded_at = DateTime.UtcNow,
                            read_value = value
                        };

                        generatedTagDataList.Add(tagData);
                    }

                    await evaluateAlarmService.EvaluateAlarms(generatedTagDataList);
                    await evaluateEventService.EvaluateEvents(generatedTagDataList);
                }

                await Task.Delay(TimeSpan.FromSeconds(10));
            }
        }


        private Dictionary<(int machineId, string tagName), float> GenerateTagValues(List<MachineTag> machineTags)
        {
            var rand = new Random();
            var tagValues = new Dictionary<(int, string), float>();

            
            var machineIds = machineTags.Select(mt => mt.machine_id).Distinct();

            foreach (var machineId in machineIds)
            {
                var tagsForMachine = machineTags.Where(mt => mt.machine_id == machineId).ToList();

                var startValue = rand.Next(0, 2);
                var stopValue = startValue == 1 ? 0 : 1;

                foreach (var tag in tagsForMachine)
                {
                    float value;

                    if (tag.tag_name.ToLower() == "start")
                        value = startValue;
                    else if (tag.tag_name.ToLower() == "stop")
                        value = stopValue;
                    else if (tag.tag_name.ToLower().Contains("bool"))
                        value = rand.Next(0, 2);
                    else
                        value = (float)(rand.NextDouble() * 150);

                    tagValues[(tag.machine_id, tag.tag_name)] = value;
                }
            }

            return tagValues;
        }


    }
}
