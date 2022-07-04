using System.Collections.Generic;
using System.Threading.Tasks;
using BoardService.Controllers;
using BoardService.Dtos;
using BoardService.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Moq;
using NUnit.Framework;

namespace BoardServiceTest
{
    [TestFixture]
    public class GetUserBoardsTest : TestableBoardController
    {
        [Test]
        public async Task GetUserBoards_Returns_200_When_User_Exists()
        {
            // Arrange
            var userId = "62a3f13a4e2288c8387a88fb";
            var user = new User
            {
                Id = userId,
                Name = "キリエライトマシュ"
            };
            mockGrpcUserClient.Setup(x => x.GetUser(userId)).Returns(user);

            var board = new Board
            {
                UserId = userId
            };

            mockBoardRepo.Setup(x => x.FindManyAsync(It.IsAny<FilterDefinition<Board>>(), null, null, null, null))
                .ReturnsAsync((1, new List<Board> { board }));

            mockMapper.Setup(x => x.Map<IEnumerable<BoardReadDto>>(It.IsAny<IEnumerable<Board>>()))
                .Returns(new List<BoardReadDto> { new BoardReadDto() });

            var controller = new BoardController(mockBoardRepo.Object, mockMapper.Object, mockGrpcUserClient.Object);

            // Act
            var result = await controller.GetUserBoards(userId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result, "Result is not 200 status code");
        }

        [Test]
        public async Task GetUserBoards_Returns_404_When_User_Not_Found()
        {
            // Arrange
            var userId = "62a3f13a4e2288c8387a88fb";
            var user = new User
            {
                Id = userId,
                Name = "キリエライトマシュ"
            };
            mockGrpcUserClient.Setup(x => x.GetUser(userId)).Returns((User)null!);

            var controller = new BoardController(mockBoardRepo.Object, mockMapper.Object, mockGrpcUserClient.Object);

            // Act
            var result = await controller.GetUserBoards(userId);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result.Result, "Result is not 404 status code");
        }

        [Test]
        public async Task GetUserBoards_Returns_400_When_UserId_Is_Null()
        {
            // Arrange
            string userId = null!;
            var controller = new BoardController(mockBoardRepo.Object, mockMapper.Object, mockGrpcUserClient.Object);

            // Act
            var result = await controller.GetUserBoards(userId);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result.Result, "Result is not 404 status code");
        }
    }
}