using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NCalc;
using System.Text.RegularExpressions;
using TrackingSystemAPI.Data;
using TrackingSystemAPI.Models.Entities;

namespace TrackingSystemAPI.BGservices
{
    public class EvaluateEventService
    {

        private readonly IServiceScopeFactory serviceScopeFactory;


        public EvaluateEventService(IServiceScopeFactory serviceScopeFactory)
        {
            this.serviceScopeFactory = serviceScopeFactory;
        }

        private List<string> ExtractTagsFromExpression(string expression)
        {
            var matches = Regex.Matches(expression, "[a-zA-Z_][a-zA-Z_09_]*");
            return matches.Cast<Match>()
                .Select(m => m.Value)
                .Distinct()
                .ToList();
        }


        public async Task EvaluateEvents(List<TagData> tagDataList)
        {
                Console.WriteLine("Evaluating events...");

                using (var scope = serviceScopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                    /*var latestTagData = await dbContext.TagDatas
                        .GroupBy(x => new { x.machine_id, x.tag_id })
                        .Select(g => g.OrderByDescending(x => x.recorded_at).FirstOrDefault())
                        .ToListAsync();*/

                    var tags = await dbContext.Tags.ToListAsync();

                    var groupedData = tagDataList
                        .Join(tags, td => td.tag_id, t => t.tag_id,
                              (td, t) => new { td.machine_id, t.tag_name, td.read_value })
                        .GroupBy(x => (x.machine_id, x.tag_name))
                        .ToDictionary(g => g.Key, g => g.First().read_value);


                    var eventTarget = await dbContext.EventTargets.ToListAsync();

                    var eventTargetGroups = eventTarget.GroupBy(e => e.event_id).ToList();


                    foreach (var group in eventTargetGroups)
                    {

                        int eventId = group.Key;

                        /*bool alreadyExists = await dbContext.EventLogs
                            .AnyAsync(x => x.event_id == eventId &&
                                        x.triggered_at > DateTime.UtcNow.AddSeconds(-10));

                        if (alreadyExists)
                        {
                            continue;
                        }*/

                        var results = new List<bool>();
                        var read_values =new List<float>();
                        var logic = new List<string>();

                        foreach (var target in group) {
                            var expr = new NCalc.Expression(target.expression);

                            expr.EvaluateParameter += (name, args) =>
                            {
                                var key = (target.machine_id, name);

                                if (groupedData.TryGetValue(key, out var val))
                                {
                                    if (Math.Abs((double)val - 0.0) < 0.0001 || Math.Abs((double)val - 1.0) < 0.0001)
                                    {
                                        args.Result = Convert.ToBoolean(val);
                                    }
                                    else
                                    {
                                        args.Result = Convert.ToDouble(val);
                                    }

                                    if (target.logic != null)
                                    {
                                        logic.Add(target.logic);
                                    }
                                }

                                else
                                {
                                    args.Result = false;
                                }

                            };

                            var result = (bool)expr.Evaluate();

                            results.Add(result);

                        /*var tagsInExpr = ExtractTagsFromExpression(target.expression);

                        var selectedTag = await (
                            from mt in dbContext.MachineTags
                            join t in dbContext.Tags on mt.tag_id equals t.tag_id
                            where mt.machine_id == target.machine_id && tagsInExpr.Contains(mt.tag_name)
                            orderby t.tag_data_type == "float" descending
                            select new { mt.tag_id, mt.machine_id }
                        ).FirstOrDefaultAsync();


                        if (selectedTag == null)
                        {
                            Console.WriteLine($"No matching tag found for EventTarget {target.event_target_id}");
                            continue; // Bu target'ı atla
                        }


                        var tagData = await dbContext.TagDatas
                            .Where(td => td.tag_id == selectedTag.tag_id && td.machine_id == selectedTag.machine_id)
                            .OrderByDescending(td => td.recorded_at)
                            .FirstOrDefaultAsync();

                        if (tagData != null)
                        {
                            read_values.Add((float)tagData.read_value);
                        }*/

                        var matching = tagDataList.FirstOrDefault(td =>
                            td.machine_id == target.machine_id &&
                            td.tag_name == ExtractTagsFromExpression(target.expression).FirstOrDefault());

                        if (matching != null)
                        {
                            read_values.Add((float)matching.read_value);
                        }


                    }

                        Console.WriteLine($"Expression results: {string.Join(", ", results)}");

                        Console.WriteLine($"Logic: {string.Join(", ", logic)}");

                        bool trigger;

                        if(logic.Any(l => l == "or"))
                        {
                            trigger = results.Any(r => r);
                        }
                        else
                        {
                            trigger = results.All(r => r);
                        }

                        if(trigger)
                        {
                            foreach (var val in read_values)
                            {
                                var eventLog = new EventLog
                                {
                                    event_id = eventId,
                                    triggered_at = DateTime.UtcNow,
                                    read_value = val
                                };
                                dbContext.EventLogs.Add(eventLog);
                            }

                            /*foreach (var data in tagDataList)
                            {
                                var newData = new TagData
                                {
                                    tag_id = data.tag_id,
                                    machine_id = data.machine_id,
                                    tag_name = data.tag_name,
                                    machine_name = data.machine_name,
                                    recorded_at = DateTime.UtcNow,
                                    read_value = data.read_value
                                };
                            }*/

                        await dbContext.SaveChangesAsync();

                            Console.WriteLine($"EVENT TRIGGERED -> EventID: {eventId}");
                        }
                    }
                }
            }

        }
    }

