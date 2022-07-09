using AutoMapper;
using BoardService.Data;
using BoardService.Services;
using Moq;
using NUnit.Framework;

namespace BoardServiceTest
{
    public class TestableBoardController
    {
        public Mock<IBoardRepo> mockBoardRepo { get; set; } = null!;
        public Mock<IMapper> mockMapper { get; set; } = null!;
        public Mock<IGrpcUserClient> mockGrpcUserClient { get; set; } = null!;

        [SetUp]
        public void Setup()
        {
            mockBoardRepo = new Mock<IBoardRepo>();
            mockMapper = new Mock<IMapper>();
            mockGrpcUserClient = new Mock<IGrpcUserClient>();
        }
    }
}