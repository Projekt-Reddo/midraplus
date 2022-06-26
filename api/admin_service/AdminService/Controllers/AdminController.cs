using AdminService.Data;
using AdminService.Dtos;
using AdminService.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace AdminService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        public ISignInRepo _signInRepo { get; }
        public AdminController(ISignInRepo signInRepo)
        {
            _signInRepo = signInRepo;
        }

        [HttpGet("dashboard/bar/{kindOfTime}")]
        public async Task<ActionResult<IEnumerable<BarChartDto>>> GetDashboardBarChart(string kindOfTime)
        {
            List<BarChartDto> barChartReturn = new List<BarChartDto>();
            switch (kindOfTime)
            {
                case "Day":
                    DateTime dayCount = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                    for (int i = 6; i >= 0; i--)
                    {
                        var filter = Builders<SignIn>.Filter.Eq("At", dayCount.AddDays(-i));
                        var day = await _signInRepo.FindOneAsync(filter: filter);
                        int value;
                        if (day == null)
                        {
                            value = 0;
                        }
                        else
                        {
                            value = day.Times;
                        }
                        barChartReturn.Add(
                            new BarChartDto
                            {
                                Index = $"{DateTime.Now.AddDays(-i).ToString("dd/MM")}",
                                Login = value,
                                LoginColor = "hsl(205, 70%, 50%)"
                            }
                        );
                    }
                    break;
                case "Month":
                    DateTime monthCount = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    for (int i = 6; i >= 0; i--)
                    {
                        var filter = Builders<SignIn>.Filter.Gte("At", monthCount.AddMonths(-i)) & Builders<SignIn>.Filter.Lte("At", monthCount.AddMonths(-i + 1));
                        (_, var day) = await _signInRepo.FindManyAsync(filter: filter);
                        var timeGet = day;
                        int timecount = 0;
                        if (timeGet == null)
                        {
                            timecount = 0;
                        }
                        else
                        {
                            foreach (var item in timeGet)
                            {
                                timecount += item.Times;
                            }
                        }
                        barChartReturn.Add(
                            new BarChartDto
                            {
                                Index = $"{DateTime.Now.AddMonths(-i).ToString("MMM")}",
                                Login = timecount,
                                LoginColor = "hsl(205, 70%, 50%)"
                            }
                        );
                    }
                    break;
                case "Year":
                    DateTime yearCount = new DateTime(DateTime.Now.Year, 1, 1);
                    for (int i = 6; i >= 0; i--)
                    {
                        (_, var day) = await _signInRepo.FindManyAsync();
                        var timeGet = day.Where(d => d.At >= yearCount.AddYears(-i) && d.At <= new DateTime(yearCount.Year - i, 12, 31));
                        int timecount = 0;
                        if (timeGet == null)
                        {
                            timecount = 0;
                        }
                        else
                        {
                            foreach (var item in timeGet)
                            {
                                timecount += item.Times;
                            }
                        }
                        barChartReturn.Add(
                            new BarChartDto
                            {
                                Index = $"{DateTime.Now.AddYears(-i).ToString("YY")}",
                                Login = timecount,
                                LoginColor = "hsl(205, 70%, 50%)"
                            }
                        );
                    }
                    break;
                default:
                    break;
            }
            return barChartReturn;
        }
    }
}