using System.Collections.Generic;
using DrawService.Dtos;
using Moq;
using NUnit.Framework;
using SignalR_UnitTestingSupport.Hubs;

namespace DrawServiceTest.ChatHubTest
{
    public class TestableChatHub : HubUnitTestsBase
    {
        public Mock<IDictionary<string, ChatConnection>> mockConnections { get; set; } = null!;

        [SetUp]
        public void Setup()
        {
            mockConnections = new Mock<IDictionary<string, ChatConnection>>();
        }
    }
}