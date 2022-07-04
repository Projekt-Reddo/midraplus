using AccountService.Data;
using AccountService.Helpers;
using AutoMapper;
using Google.Apis.Auth;
using Moq;
using NUnit.Framework;

namespace DraplusApiTest.Controllers.UserControllerTest
{
    public class TestableUserController
    {
        public Mock<IUserRepo> mockUserRepo { get; set; } = null!;
        public Mock<IMapper> mockMapper { get; set; } = null!;
        public Mock<IJwtGenerator> mockJwtGenerator { get; set; } = null!;
        public Mock<GoogleJsonWebSignature> mockGoogleJson { get; set; } = null!;

        [SetUp]
        public void Setup()
        {
            // Mock controller dependencies
            mockUserRepo = new Mock<IUserRepo>();
            mockMapper = new Mock<IMapper>();
            mockJwtGenerator = new Mock<IJwtGenerator>();
            mockGoogleJson = new Mock<GoogleJsonWebSignature>();
        }
    }
}