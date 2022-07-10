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
    public class DeleteBoardTest : TestableBoardController
    {
        [Test]
        public async Task DeleteBoard_Return_200_When_Board_Existed()
        {
            var boardId = "62a3f13a4e2288c8387a88fb";
            mockBoardRepo.Setup(b => b.DeleteOneAsync(boardId)).ReturnsAsync(true);

            var controller = new BoardController(mockBoardRepo.Object, mockMapper.Object, mockGrpcUserClient.Object);

            var result = await controller.DeleteBoard(boardId);

            Assert.IsInstanceOf<OkObjectResult>(result.Result, "Result is not 200 status code");
        }
        [Test]
        public async Task DeleteBoard_Return_404_When_Board_Not_Existed()
        {
            var boardId = "62a3f13a4e2288c8387a88fb";
            mockBoardRepo.Setup(b => b.DeleteOneAsync(boardId)).ReturnsAsync(false);

            var controller = new BoardController(mockBoardRepo.Object, mockMapper.Object, mockGrpcUserClient.Object);

            var result = await controller.DeleteBoard(boardId);

            Assert.IsInstanceOf<NotFoundObjectResult>(result.Result, "Board not found");
        }
    }
}