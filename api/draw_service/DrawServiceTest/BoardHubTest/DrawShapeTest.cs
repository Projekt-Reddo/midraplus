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
    public class DrawShapeTest : TestableBoardHub
    {
        [Test]
        public async Task DrawShape_Success_LinePathData()
        {
            // Arrange
            var boardHub = new BoardHub(
                connections: mockConnections.Object,
                mapper: mockMapper.Object,
                shapeList: mockShapeList.Object,
                noteList: mockNoteList.Object,
                grpcBoardClient: mockGrpcBoardClient.Object);

            AssignToHubRequiredProperties(boardHub); // Resolve hub dependencies as IClientsProxy...

            DrawConnection userConnection = new DrawConnection { BoardId = "629733ed351648037cb155fe" };
            mockConnections.Setup(connections => connections.TryGetValue(It.IsAny<string>(), out userConnection!)).Returns(true); // ! after userConnection is for null forgiving

            mockShapeList.Setup(list => list[It.IsAny<string>()]).Returns(new List<ShapeReadDto>()); // ! after shape is for null forgiving

            var shape = new ShapeReadDto { Data = new List<string> { "1", "2", "3", "4" } };

            // Act
            await boardHub.DrawShape(shape);

            // Assert
            ClientsOthersInGroupMock.Verify(x => x.SendCoreAsync(HubReturnMethod.ReceiveShape, new object[] { shape }, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task DrawShape_Fail_NoUserInConnections()
        {
            // Arrange
            var boardHub = new BoardHub(
                connections: mockConnections.Object,
                mapper: mockMapper.Object,
                shapeList: mockShapeList.Object,
                noteList: mockNoteList.Object,
                grpcBoardClient: mockGrpcBoardClient.Object);

            AssignToHubRequiredProperties(boardHub); // Resolve hub dependencies as IClientsProxy...

            DrawConnection userConnection = new DrawConnection { BoardId = "629733ed351648037cb155fe" };
            mockConnections.Setup(connections => connections.TryGetValue(It.IsAny<string>(), out userConnection!)).Returns(false); // ! after userConnection is for null forgiving

            var note = new ShapeReadDto { Data = new List<string> { "1", "2", "3", "4" } };

            // Act
            await boardHub.DrawShape(note);

            // Assert
            ClientsOthersInGroupMock.Verify(x => x.SendCoreAsync(HubReturnMethod.ReceiveShape, new object[] { note }, It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}