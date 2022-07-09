using AdminService.Dtos;
using AutoMapper;
using BoardService;
using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;

namespace AdminService.Services
{
    public interface IGrpcBoardClient
    {
        List<BoardLoadByTime> LoadBoardListByTime(DateTime startDate, DateTime endDate);
        TotalBoardsRespone GetTotalBoards();
    }

    public class GrpcBoardClient : IGrpcBoardClient
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        private readonly GrpcBoard.GrpcBoardClient _client;

        public GrpcBoardClient(IConfiguration configuration, IMapper mapper, ILogger<GrpcBoardClient> logger)
        {
            _configuration = configuration;
            _mapper = mapper;
            _logger = logger;

            string artistServerUrl = _configuration.GetValue<string>("Grpc:Board");

            var channel = GrpcChannel.ForAddress(artistServerUrl);
            _client = new GrpcBoard.GrpcBoardClient(channel);
        }

        public List<BoardLoadByTime> LoadBoardListByTime(DateTime startDate, DateTime endDate)
        {
            var request = new BoardLoadByTimeRequest
            {
                StartDate = (DateTime.SpecifyKind(startDate, DateTimeKind.Utc)).ToTimestamp(),
                EndDate = (DateTime.SpecifyKind(endDate, DateTimeKind.Utc)).ToTimestamp()
            };

            try
            {
                var response = _client.LoadBoardListByTime(request);

                var rs = _mapper.Map<List<BoardLoadByTime>>(response.BoardList.ToList());

                return rs;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while getting board from {startDate} to {endDate}");
                return null;
            }
        }
        public TotalBoardsRespone GetTotalBoards()
        {
            try
            {
                var response = _client.GetTotalBoard(new GetTotalBoardsRequest());
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while getting total boards");
                return null;
            }
<<<<<<< HEAD
            
=======

>>>>>>> d1bfab64153efc8690f88ad49e004ef604cd7042
        }
    }
}