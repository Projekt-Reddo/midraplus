using System.Collections.Generic;
using System.Threading.Tasks;
using AccountService.Controllers;
using AccountService.Dtos;
using AccountService.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Moq;
using NUnit.Framework;

namespace DraplusApiTest.Controllers.UserControllerTest
{
    public class GetUserList : TestableUserController
    {
        [Test]
        public async Task GetUserList_NormalFlow_ReturnOK()
        {
            // Arrange
            var userController = new UserController(mockUserRepo.Object, mockMapper.Object);

            var pagiParam = new PaginationParameterDto()
            {
                PageNumber = 1,
                PageSize = 10,
                SearchName = ""
            };

            mockUserRepo.Setup(x => x.FindManyAsync(It.IsAny<FilterDefinition<User>>(), null, null, null, null))
                .ReturnsAsync((10, (IEnumerable<User>)null!));

            mockUserRepo.Setup(x => x.FindManyAsync(It.IsAny<FilterDefinition<User>>(), null, null, It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync((10, (IEnumerable<User>)null!));

            // Act
            var result = await userController.GetAllUserManage(pagiParam);

            // Assert
            Assert.IsInstanceOf(typeof(OkObjectResult), result.Result);
        }

        [Test]
        public async Task GetUserList_Search_ReturnOK()
        {
            // Arrange
            var userController = new UserController(mockUserRepo.Object, mockMapper.Object);

            var pagiParam = new PaginationParameterDto()
            {
                PageNumber = 1,
                PageSize = 10,
                SearchName = "hoangvi"
            };

            mockUserRepo.Setup(x => x.FindManyAsync(It.IsAny<FilterDefinition<User>>(), null, null, null, null))
                .ReturnsAsync((10, (IEnumerable<User>)null!));

            mockUserRepo.Setup(x => x.FindManyAsync(It.IsAny<FilterDefinition<User>>(), null, null, It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync((10, (IEnumerable<User>)null!));

            // Act
            var result = await userController.GetAllUserManage(pagiParam);

            // Assert
            Assert.IsInstanceOf(typeof(OkObjectResult), result.Result);
        }
    }
}