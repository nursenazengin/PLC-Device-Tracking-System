using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NCalc;
using System.Text.RegularExpressions;
using TrackingSystemAPI.Data;
using TrackingSystemAPI.Models.Entities;

namespace TrackingSystemAPI.BGservices
{
    public class EvaluateAlarmService
    {

        private readonly IServiceScopeFactory serviceScopeFactory;

        public EvaluateAlarmService(IServiceScopeFactory serviceScopeFactory)
        {
            this.serviceScopeFactory = serviceScopeFactory;
        }

        private List<string> ExtractTagsFromExpression(string expr)
        {
            var matches = Regex.Matches(expr, "[a-zA-Z_][a-zA-Z0-9_]*");
            return matches.Cast<Match>().Select(m => m.Value).Distinct().ToList();
        }


        public async Task EvaluateAlarms(List<TagData> tagDataList)
        {

            using (var scope = serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var alarmTarget = await dbContext.AlarmTargets.ToListAsync();
                var tags = await dbContext.Tags.ToListAsync();

                var groupedData = tagDataList
                    .GroupBy(td => (td.machine_id, td.tag_name))
                    .ToDictionary(g => g.Key, g => g.First().read_value);

                var alarmTargetGroups = alarmTarget.GroupBy(at => at.alarm_id).ToList();

                foreach (var group in alarmTargetGroups)
                {
                    int alarmId = group.Key;

                    var results = new List<bool>();
                    var read_values = new List<float>();
                    var logic = new List<string>();

                    foreach (var target in group)
                    {
                        var expr = new NCalc.Expression(target.expression);

                        expr.EvaluateParameter += (name, args) =>
                        {
                            var key = (target.machine_id, name);

                            if (groupedData.TryGetValue(key, out var val))
                            {
                                if (Math.Abs((double)val - 0.0) < 0.0001 || Math.Abs((double)val - 1.0) < 0.0001)
                                    args.Result = Convert.ToBoolean(val);
                                else
                                    args.Result = Convert.ToDouble(val);
                            }
                            else
                            {
                                args.Result = false;
                            }

                            if (target.logic != null)
                            {
                                logic.Add(target.logic);
                            }
                        };

                        var result = (bool)expr.Evaluate();
                        results.Add(result);

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

                    if (logic.Any(l => l == "or") || logic.Count == 0)
                    {
                        trigger = results.Any(r => r);
                    }
                    else 
                    {
                        trigger = results.All(r => r);
                    }

                    if (trigger)
                    {
                        foreach (var val in read_values)
                        {
                            var alarmLog = new AlarmLog
                            {
                                alarm_id = alarmId,
                                read_value = val,
                                triggered_at = DateTime.UtcNow
                            };
                            dbContext.AlarmLogs.Add(alarmLog);
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
                        Console.WriteLine($"ALARM TRIGGERED -> AlarmID: {alarmId}");

                    }
                }

            }



        }
    }
}



