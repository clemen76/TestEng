using System;
using System.Threading.Tasks;
using Xunit;
using Moq;
using TestEng.Controllers;
using TestEng.AppServices.Services;
using TestEng.Data.Models;
using TestEng.Entities.Repositories;

namespace TestEng.Unit_Tests.Tests
{
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClass()]
    public class UserControllerTests
    {
        Mock<IUserService> _userServicesMock;
        Mock<IUserRepository> _userRepositoryMock;

        // SUT
        private readonly UserController _userController;

        public UserControllerTests()
        {
            _userController = CreateUserController();
        }

        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod()]
        [Fact]
        public void Edit()
        {
            // Arrange
            var model = new UserModel() { UserId = 1, BirthDate = DateTime.UtcNow, IsActive = true, Name = "Clemente" };
            _userRepositoryMock.Setup(m => m.GetById(It.IsAny<int>()))
                              .Returns(Task.FromResult(model));

            // Act
            var result = _userController.Edit(1);

            // Assert
            Assert.NotNull(result);
        }

        private UserController CreateUserController()
        {
            _userServicesMock = new Mock<IUserService>();
            _userRepositoryMock = new Mock<IUserRepository>();

            return new UserController(_userServicesMock.Object);
        }
    }
}