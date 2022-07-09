using AdminService.Data;
using AdminService.Services;
using AutoMapper;
using Moq;
using NUnit.Framework;

namespace AdminServiceTest
{
    public class TestableAdminController
    {
        public Mock<ISignInRepo> mockSignInRepo { get; set; } = null!;
        public Mock<IMapper> mockMapper { get; set; } = null!;
        public Mock<IGrpcBoardClient> mockGrpcBoardClient { get; set; } = null!;

        [SetUp]
        public void Setup()
        {
            mockSignInRepo = new Mock<ISignInRepo>();
            mockMapper = new Mock<IMapper>();
            mockGrpcBoardClient = new Mock<IGrpcBoardClient>();
        }
    }
}