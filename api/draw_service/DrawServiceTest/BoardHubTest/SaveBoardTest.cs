using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DrawService.Dtos;
using DrawService.Hubs;
using Moq;
using NUnit.Framework;
using static DrawService.Helpers.Constant;

namespace DrawServiceTest.BoardHubTest
{
    public class SaveBoardTest : TestableBoardHub
    {
        [Test]
        public async Task LeaveBoard_WhenCalled()
        {
            // Arrange
            var boardHub = new BoardHub(
                connections: mockConnections.Object,
                mapper: mockMapper.Object,
                shapeList: mockShapeList.Object,
                noteList: mockNoteList.Object,
                grpcBoardClient: mockGrpcBoardClient.Object);

            AssignToHubRequiredProperties(boardHub); // Resolve hub dependencies as IClientsProxy...

            await boardHub.LeaveRoom();

            Assert.Pass();
        }
    }
}