using BoardService.Controllers;
using BoardService.Dtos;
using BoardService.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoardServiceTest
{
    public class AddBoardTest : TestableBoardController
    {
        [Test]
        public async Task AddBoard_UserIdIsNull_ReturnsBadRequest()
        {
            // Arrange
            var boardController = new BoardController(mockBoardRepo.Object,mockMapper.Object, mockGrpcUserClient.Object);
            var boardCreateDto = new BoardCreateDto
            {
                UserId = null!,
            };

            // Act
            var result = await boardController.AddBoard(boardCreateDto);
            
            // Assert
            Assert.IsInstanceOf(typeof(ActionResult<ResponseDto>), result);
            Assert.IsInstanceOf(typeof(BadRequestObjectResult), result.Result);
        }

        [Test]
        public async Task AddBoard_UserIdIsNotExisted_ReturnsNotFound()
        {
            // Arrange
            var boardController = new BoardController(mockBoardRepo.Object, mockMapper.Object, mockGrpcUserClient.Object);
            var boardCreateDto = new BoardCreateDto
            {
                UserId = "62aee6ba5159f89608499e70adas",
            };

            mockGrpcUserClient.Setup(x => x.GetUser(boardCreateDto.UserId)).Returns((User)null!);

            // Act
            var result = await boardController.AddBoard(boardCreateDto);

            // Assert
            Assert.IsInstanceOf(typeof(ActionResult<ResponseDto>), result);
            Assert.IsInstanceOf(typeof(NotFoundObjectResult), result.Result);
        }

        [Test]
        public async Task AddBoard_UserIdIsNotNull_ReturnsOk()
        {
            // Arrange
            var boardController = new BoardController(mockBoardRepo.Object, mockMapper.Object, mockGrpcUserClient.Object);
            var boardCreateDto = new BoardCreateDto
            {
                UserId = "62aee6ba5159f89608499e70",
            };

            var user = new User{
                Id = "62aee6ba5159f89608499e70",
                Name = "Viroku"
            };

            mockGrpcUserClient.Setup(x => x.GetUser(boardCreateDto.UserId)).Returns(user);

            mockBoardRepo.Setup(x => x.AddOneAsync(It.IsAny<Board>()))
                .ReturnsAsync(new Board());

            // Act
            var result = await boardController.AddBoard(boardCreateDto);

            // Assert
            Assert.IsInstanceOf(typeof(ActionResult<ResponseDto>), result);
            Assert.IsInstanceOf(typeof(OkObjectResult), result.Result);
        }
    }
}