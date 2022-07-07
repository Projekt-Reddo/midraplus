using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DrawService.Dtos;
using DrawService.Hubs;
using Microsoft.AspNetCore.SignalR;
using Moq;
using NUnit.Framework;
using static DrawService.Helpers.Constant;

namespace DrawServiceTest.ChatHubTest
{
    [TestFixture]
    public class JoinLeaveTest : TestableChatHub
    {
        [Test]
        public async Task JoinRoom_WhenCalled()
        {
            var chatHub = new ChatHub(
                connections: mockConnections.Object
            );
            AssignToHubRequiredProperties(chatHub);

            var user = new UserConnectionInfo
            {
                Id = "62a3f13a4e2288c8387a88fb",
                Name = "キリエライトマシュ",
                Avatar = "url"
            };

            ChatConnection userConnection = new ChatConnection
            {
                BoardId = "629733ed351648037cb155fe",
                User = user
            };

            mockConnections.Setup(connections => connections.TryGetValue(It.IsAny<string>(), out userConnection!)).Returns(true);

            // Mock<HubCallerContext> mockClientContext = new Mock<HubCallerContext>();
            // mockClientContext.Setup(c => c.ConnectionId).Returns("clientId");
            // chatHub.Context = mockClientContext.Object;

            // Act
            await chatHub.JoinRoom(userConnection);
            await chatHub.SendMessage(user, "message");

            ClientsGroupMock.Verify(clients =>
                clients.SendCoreAsync(HubReturnMethod.ReceiveMessage,
                                      It.IsAny<object[]>(),
                                      It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task LeaveRoom_WhenCalled()
        {
            var chatHub = new ChatHub(
               connections: mockConnections.Object
           );
            AssignToHubRequiredProperties(chatHub);

            var user = new UserConnectionInfo
            {
                Id = "62a3f13a4e2288c8387a88fb",
                Name = "キリエライトマシュ",
                Avatar = "url"
            };

            ChatConnection userConnection = new ChatConnection
            {
                BoardId = "629733ed351648037cb155fe",
                User = user
            };

            mockConnections.Setup(connections => connections.TryGetValue(It.IsAny<string>(), out userConnection!)).Returns(true);

            // Mock<HubCallerContext> mockClientContext = new Mock<HubCallerContext>();
            // mockClientContext.Setup(c => c.ConnectionId).Returns("clientId");
            // chatHub.Context = mockClientContext.Object;

            // Act
            await chatHub.JoinRoom(userConnection);
            await chatHub.LeaveRoom();

            Assert.Pass();
        }
    }
}