using AdminService.Data;
using AdminService.Dtos;
using AdminService.Models;
using AdminService.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace AdminService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        public ISignInRepo _signInRepo { get; }
        private readonly IGrpcUserClient _grpcUserClient;

        private readonly IGrpcBoardClient _grpcBoardClient;

        public AdminController(ISignInRepo signInRepo, IGrpcBoardClient grpcBoardClient, IGrpcUserClient grpcUserClient)
        {
            _signInRepo = signInRepo;
            _grpcBoardClient = grpcBoardClient;
            _grpcUserClient = grpcUserClient;
        }
        /// <summary>
        /// Get Dashboard
        /// </summary>
        /// <returns></returns>
        [HttpGet("dashboard/Sumarize")]
        public async Task<ActionResult<SumarizeDto>> GetDashboardDetails()
        {
            var userList = _grpcUserClient.GetTotalAccount();
            var totalMem = userList.Total;
            var boardList = _grpcBoardClient.GetTotalBoards();

            SumarizeDto returnValue = new SumarizeDto
            {
                NewAccount = userList.Account7Days,
                NewBoard = boardList.Total,
                TotalAccount = totalMem,
                TotalBoard = boardList.Boards7Days,
            };
            return Ok(returnValue);
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

        /// <summary>
        /// Number of Board report
        /// </summary>
        /// <param name="kindOfTime"></param>
        /// <returns></returns>
        [HttpGet("dashboard/line/{kindOfTime}")]
        public async Task<ActionResult<IEnumerable<LineChartDto>>> GetDashBoardLineChar(string kindOfTime)
        {
            LineChartDto returnLineChar = new LineChartDto();
            List<LineData> lineDatas = new List<LineData>();
            switch (kindOfTime)
            {
                case "Day":
                    {
                        // Get all boards that created from end to start (by day)
                        DateTime startDate = DateTime.Now.AddDays(-8);
                        DateTime endDate = DateTime.Now.AddDays(1);

                        var boards = _grpcBoardClient.LoadBoardListByTime(startDate, endDate);

                        int todayDate = DateTime.Now.Day;

                        for (int i = 6; i >= 0; i--)
                        {
                            var dayToCompare = DateTime.Now.AddDays(-i);

                            var numberOfBoardCreatedInDayI = boards.FindAll(b => b.CreatedAt.Day == dayToCompare.Day).Count;

                            lineDatas.Add(
                                new LineData
                                {
                                    x = $"{dayToCompare.Day}",
                                    y = numberOfBoardCreatedInDayI,
                                }
                            );
                        }
                        break;
                    }

                case "Month":
                    {
                        // Get all boards that created from end to start (by month)
                        DateTime startDate = DateTime.Now.AddMonths(-8);
                        DateTime endDate = DateTime.Now.AddMonths(1);

                        var boards = _grpcBoardClient.LoadBoardListByTime(startDate, endDate);

                        int todayDate = DateTime.Now.Month;

                        for (int i = 6; i >= 0; i--)
                        {
                            var dayToCompare = DateTime.Now.AddMonths(-i);

                            var numberOfBoardCreatedInMonthI = boards.FindAll(b => b.CreatedAt.Month == dayToCompare.Month).Count;

                            lineDatas.Add(
                                new LineData
                                {
                                    x = $"{dayToCompare.Month}",
                                    y = numberOfBoardCreatedInMonthI,
                                }
                            );
                        }
                        break;
                    }

                case "Year":
                    {
                        // Get all boards that created from end to start (by year)
                        DateTime startDate = DateTime.Now.AddYears(-8);
                        DateTime endDate = DateTime.Now.AddYears(1);

                        var boards = _grpcBoardClient.LoadBoardListByTime(startDate, endDate);

                        int todayDate = DateTime.Now.Year;

                        for (int i = 6; i >= 0; i--)
                        {
                            var dayToCompare = DateTime.Now.AddYears(-i);

                            var numberOfBoardCreatedInYearI = boards.FindAll(b => b.CreatedAt.Year == dayToCompare.Year).Count;

                            lineDatas.Add(
                                new LineData
                                {
                                    x = $"{dayToCompare.Year}",
                                    y = numberOfBoardCreatedInYearI,
                                }
                            );
                        }
                        break;
                    }

                default:
                    break;
            }
            returnLineChar.Id = kindOfTime;
            returnLineChar.Color = "hsl(205, 70%, 50%)";
            returnLineChar.Data = lineDatas;
            List<LineChartDto> addLineChart = new List<LineChartDto>();
            addLineChart.Add(returnLineChar);
            return addLineChart;
        }
    }
}