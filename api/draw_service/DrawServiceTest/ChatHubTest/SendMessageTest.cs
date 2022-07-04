using System.Threading;
using System.Threading.Tasks;
using DrawService.Dtos;
using DrawService.Hubs;
using Moq;
using NUnit.Framework;
using static DrawService.Helpers.Constant;

namespace DrawServiceTest.ChatHubTest
{
    [TestFixture]
    public class SendMessageTest : TestableChatHub
    {
        [Test]
        public async Task SendMessage_WhenCalled_SendMessageToEveryoneInGroup()
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

            // Act
            await chatHub.SendMessage(user, "message");

            // Assert
            ClientsGroupMock.Verify(clients =>
                clients.SendCoreAsync(HubReturnMethod.ReceiveMessage,
                                      It.IsAny<object[]>(),
                                      It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task SendMessage_WhenCalled_NotSendMessageToOutsideGroup()
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

            mockConnections.Setup(connections => connections.TryGetValue(It.IsAny<string>(), out userConnection!)).Returns(false);

            // Act
            await chatHub.SendMessage(user, "message");

            // Assert
            ClientsGroupMock.Verify(clients =>
                clients.SendCoreAsync(HubReturnMethod.ReceiveMessage,
                                      It.IsAny<object[]>(),
                                      It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}