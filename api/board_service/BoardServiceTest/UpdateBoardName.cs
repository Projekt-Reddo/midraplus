using System;
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
    public class UpdateBoardNameTest : TestableBoardController
    {
        [Test]
        public async Task UpdateBoardName_Returns_200_When_Board_Exists()
        {
            // Arrange
            var boardId = "629733ed351648037cb155fe";

            var board = new Board
            {
                Id = boardId,
                Name = "Sui-chan wa kyou mo kawai",
                CreatedAt = DateTime.Now.AddDays(-10),
                LastEdit = DateTime.Now.AddDays(-7),
                UserId = "62972e6aeb459943fe8796d6"
            };

            mockBoardRepo.Setup(x => x.FindOneAsync(It.IsAny<FilterDefinition<Board>>()))
               .ReturnsAsync(board);

            mockBoardRepo.Setup(x => x.UpdateOneAsync("629733ed351648037cb155fe", It.IsAny<Board>()))
                .ReturnsAsync(true);

            var controller = new BoardController(mockBoardRepo.Object, mockMapper.Object, mockGrpcUserClient.Object);

            // Act
            var result = await controller.UpdateBoardName(boardId, new BoardUpdateDto
            {
                Id = boardId,
                Name = "Suicopath"
            });

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result, "Result is not 200 status code");
        }

        [Test]
        public async Task UpdateBoardName_Returns_404_When_Board_Not_Exist()
        {
            // Arrange
            var boardId = "629733ed351648037cb155fe";
            var boardIdNotExist = "629733ed351648037cb155ff";

            var board = new Board
            {
                Id = boardId,
                Name = "Stellar Stellar",
                CreatedAt = DateTime.Now.AddDays(-10),
                LastEdit = DateTime.Now.AddDays(-7),
                UserId = "62972e6aeb459943fe8796d6"
            };

            mockBoardRepo.Setup(x => x.FindOneAsync(Builders<Board>.Filter.Eq("Id", boardIdNotExist)))
               .ReturnsAsync((Board)null!);

            var controller = new BoardController(mockBoardRepo.Object, mockMapper.Object, mockGrpcUserClient.Object);

            // Act
            var result = await controller.UpdateBoardName(boardId, new BoardUpdateDto
            {
                Id = boardId,
                Name = "IRyS love Bae forever"
            });

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result.Result, "Result is not 404 status code");
        }

        [Test]
        public async Task ChangeName_EmptyStringName_ReturnsBadRequest()
        {
            // Arrange
            var boardController = new BoardController(mockBoardRepo.Object, mockMapper.Object, mockGrpcUserClient.Object);
            var boardForChangeNameDto = new BoardUpdateDto
            {
                Id = "629733ed351648037cb155fe",
                Name = ""
            };

            mockBoardRepo.Setup(x => x.FindOneAsync(It.IsAny<FilterDefinition<Board>>()))
                .ReturnsAsync(new Board());

            mockBoardRepo.Setup(x => x.UpdateOneAsync("629733ed351648037cb155fe", It.IsAny<Board>()))
                .ReturnsAsync(false);

            // Act
            var result = await boardController.UpdateBoardName("629733ed351648037cb155fe", boardForChangeNameDto);

            // Assert
            Assert.IsInstanceOf(typeof(ActionResult<ResponseDto>), result);
            Assert.IsInstanceOf(typeof(BadRequestObjectResult), result.Result);
        }
    }
}