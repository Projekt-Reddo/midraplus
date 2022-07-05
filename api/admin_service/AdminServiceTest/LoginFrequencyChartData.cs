using System.Collections.Generic;
using System.Threading.Tasks;
using AdminService.Controllers;
using AdminService.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Moq;
using NUnit.Framework;

namespace AdminServiceTest
{
    public class LoginFrequencyChartData : TestableAdminController
    {
        [Test]
        public async Task LoginFrequency_Day_ReturnOK()
        {
            // Arrange
            var adminController = new AdminController(mockSignInRepo.Object, mockGrpcBoardClient.Object);

            mockSignInRepo.Setup(x => x.FindOneAsync(It.IsAny<FilterDefinition<SignIn>>()))
                .ReturnsAsync((SignIn)null!);

            var kindOfTime = "Day";

            // Act
            var result = await adminController.GetDashboardBarChart(kindOfTime);

            // Assert
            Assert.IsInstanceOf(typeof(OkObjectResult), result.Result);
        }

        [Test]
        public async Task LoginFrequency_Month_ReturnOK()
        {
            // Arrange
            var adminController = new AdminController(mockSignInRepo.Object, mockGrpcBoardClient.Object);

            mockSignInRepo.Setup(x => x.FindManyAsync(It.IsAny<FilterDefinition<SignIn>>(), null, null, null, null))
                .ReturnsAsync((10, (IEnumerable<SignIn>)null!));

            var kindOfTime = "Month";

            // Act
            var result = await adminController.GetDashboardBarChart(kindOfTime);

            // Assert
            Assert.IsInstanceOf(typeof(OkObjectResult), result.Result);
        }

        [Test]
        public async Task LoginFrequency_Year_ReturnOK()
        {
            // Arrange
            var adminController = new AdminController(mockSignInRepo.Object, mockGrpcBoardClient.Object);

            mockSignInRepo.Setup(x => x.FindManyAsync(null!, null, null, null, null))
                .ReturnsAsync((10, (IEnumerable<SignIn>)null!));

            var kindOfTime = "Year";

            // Act
            var result = await adminController.GetDashboardBarChart(kindOfTime);

            // Assert
            Assert.IsInstanceOf(typeof(OkObjectResult), result.Result);
        }

        [Test]
        public async Task LoginFrequency_None_ReturnOK()
        {
            // Arrange
            var adminController = new AdminController(mockSignInRepo.Object, mockGrpcBoardClient.Object);

            mockSignInRepo.Setup(x => x.FindOneAsync(It.IsAny<FilterDefinition<SignIn>>()))
                .ReturnsAsync((SignIn)null!);

            var kindOfTime = "";

            // Act
            var result = await adminController.GetDashboardBarChart(kindOfTime);

            // Assert
            Assert.IsInstanceOf(typeof(OkObjectResult), result.Result);
        }
    }
}