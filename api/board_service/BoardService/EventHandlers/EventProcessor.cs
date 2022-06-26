using BoardService.Data;
using BoardService.Dtos;
using BoardService.Models;
using System.Text.Json;

namespace BoardService.EventHandlers
{
    public interface IEventProcessor
    {
        Task ProcessEvent(string message);
    }
    public class EventProcessor : IEventProcessor
    {
        private readonly IBoardRepo _repository;
        private readonly ILogger<EventProcessor> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public EventProcessor(IBoardRepo repository,
            ILogger<EventProcessor> logger,
            IServiceScopeFactory serviceScopeFactory
        )
        {
            _repository = repository;
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }
        public async Task ProcessEvent(string message)
        {
            var eventType = DetermineEventType(message);
            switch (eventType)
            {
                case EventType.CreateBoard: // create default board for new signin account
                    await ProcessCreateBoard(message);
                    break;

                default:
                    _logger.LogWarning("Going default Event");
                    break;
            }
        }

        private async Task ProcessCreateBoard(string message)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var createBoardEventDto = JsonSerializer.Deserialize<MessageCreateBoardSubscribeDto>(message);
                try
                {
                    // Create new chat room & board
                    var createdBoard = await _repository.AddOneAsync(new Board
                    {
                        UserId = createBoardEventDto!.Id,
                        Name = createBoardEventDto.Name,
                    });
                    _logger.LogInformation($"Create a default board {createBoardEventDto.Name}");
                }
                catch
                {
                    _logger.LogWarning($"Failed to a default board {createBoardEventDto!.Name}");
                }
            }
        }

        private EventType DetermineEventType(string message)
        {
            var eventType = JsonSerializer.Deserialize<MessageCreateBoardSubscribeDto>(message);

            switch (eventType!.Event)
            {
                case "CreateBoard":
                    return EventType.CreateBoard;

                default:
                    return EventType.Unknown;
            }
        }
    }
}
enum EventType
{
    CreateBoard,
    Unknown
}