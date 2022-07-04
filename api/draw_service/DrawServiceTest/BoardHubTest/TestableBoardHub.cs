using System.Collections.Generic;
using AutoMapper;
using DrawService.Dtos;
using DrawService.Services;
using Moq;
using NUnit.Framework;
using SignalR_UnitTestingSupport.Hubs;

namespace DrawServiceTest.BoardHubTest
{
    public class TestableBoardHub : HubUnitTestsBase
    {
        public Mock<IDictionary<string, DrawConnection>> mockConnections { get; set; } = null!;
        public Mock<IMapper> mockMapper { get; set; } = null!;
        public Mock<IDictionary<string, ICollection<ShapeReadDto>>> mockShapeList { get; set; } = null!;
        public Mock<IDictionary<string, List<NoteReadDto>>> mockNoteList { get; set; } = null!;
        public Mock<IGrpcBoardClient> mockGrpcBoardClient { get; set; } = null!;

        [SetUp]
        public void Setup()
        {
            mockConnections = new Mock<IDictionary<string, DrawConnection>>();
            mockMapper = new Mock<IMapper>();
            mockShapeList = new Mock<IDictionary<string, ICollection<ShapeReadDto>>>();
            mockNoteList = new Mock<IDictionary<string, List<NoteReadDto>>>();
            mockGrpcBoardClient = new Mock<IGrpcBoardClient>();
        }
    }
}