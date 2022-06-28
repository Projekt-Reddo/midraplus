using System.Collections.Generic;
using DrawService.Dtos;
using Moq;
using NUnit.Framework;

namespace DrawServiceTest.ChatHubTest
{
    public class TestableChatHub
    {
        public Mock<Dictionary<string, ChatConnection>> mockConnections { get; set; } = null!;

        [SetUp]
        public void Setup()
        {
            mockConnections = new Mock<Dictionary<string, ChatConnection>>();
        }
    }
}