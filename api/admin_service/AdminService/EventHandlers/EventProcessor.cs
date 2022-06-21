using AdminService.Data;
using AdminService.Dtos;
using AdminService.Models;
using MongoDB.Driver;
using System.Text.Json;

namespace AdminService.EventHandlers
{

    public interface IEventProcessor
    {
        Task ProcessEvent(string message);
    }
    public class EventProcessor : IEventProcessor
    {
        private readonly ISignInRepo _repository;
        private readonly ILogger<EventProcessor> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public EventProcessor(ISignInRepo repository,
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
                case EventType.AddSignIn: // increase signin count
                    await ProcessAddSignIn(message);
                    break;

                default:
                    _logger.LogWarning("Going default Event");
                    break;
            }
        }

        private async Task ProcessAddSignIn(string message)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                DateTime currentDay = new DateTime(
                                DateTime.Now.Year,
                                DateTime.Now.Month,
                                DateTime.Now.Day);
                var filter = Builders<SignIn>.Filter.Eq("At", currentDay);
                var sigin = await _repository.FindOneAsync(filter);

                if (sigin == null) // if there is no signin count for this day. Create new
                {
                    var createdSignIn = await _repository.AddOneAsync(new SignIn()
                    {
                        At = currentDay,
                        Times = 1
                    });

                    _logger.LogInformation($"Create a new sign in at {currentDay.ToString()}");
                }
                else
                { // increase count +1 
                    sigin.Times += 1;
                    var rs = await _repository.UpdateOneAsync(sigin.Id, sigin);

                    _logger.LogInformation($"Update a new sign in at ${currentDay.ToString()}: {rs}");
                }
            }
        }

        private EventType DetermineEventType(string message)
        {
            var eventType = JsonSerializer.Deserialize<EventDto>(message);

            switch (eventType!.Event)
            {
                case "AddSignIn":
                    return EventType.AddSignIn;

                default:
                    return EventType.Unknown;
            }
        }
    }
}

enum EventType
{
    AddSignIn,
    Unknown
}
