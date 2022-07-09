using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccountService.Controllers;
using AccountService.Models;
using DraplusApiTest.Controllers.UserControllerTest;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Moq;
using NUnit.Framework;

namespace AccountServiceTest
{
    public class BanUserTest : TestableUserController
    {
        [Test]
        public async Task BanUser_UserIdIsNull_ReturnNotFound()
        {
            // Arrange
            var userController = new UserController(mockUserRepo.Object, mockMapper.Object);


            string UserId = null!;

            mockUserRepo.Setup(x => x.FindOneAsync(It.IsAny<FilterDefinition<User>>()))
                .ReturnsAsync(((User)null!));

            // // Act
            var result = await userController.BanUser(UserId);

            // // Assert
            Assert.IsInstanceOf(typeof(NotFoundObjectResult), result);
        }

        [Test]
        public async Task BanUser_UserIdIsNotNull_ReturnOk()
        {
            // Arrange
            var userController = new UserController(mockUserRepo.Object, mockMapper.Object);


            string UserId = "62aee6ba5159f89608499e70";

            var user = new User
            {
                Id = UserId
            };

            mockUserRepo.Setup(x => x.FindOneAsync(It.IsAny<FilterDefinition<User>>()))
                .ReturnsAsync(user);

            mockUserRepo.Setup(x => x.UpdateOneAsync(UserId, It.IsAny<User>()))
                .ReturnsAsync(true);
            // // Act
            var result = await userController.BanUser(UserId);

            // // Assert
            Assert.IsInstanceOf(typeof(OkResult), result);
        }
    }
}