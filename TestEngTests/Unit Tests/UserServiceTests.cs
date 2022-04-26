using System;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Moq;
using TestEng.AppServices.Services;
using TestEng.Data.Models;
using TestEng.Entities.Repositories;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestEngTests.Unit_Tests
{
    [TestClass]
    public class UserServiceTests
    {
        Mock<IUserRepository> _userRepositoryMock;

        // SUT
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _userService = CreateUserService();
        }

        #region GetUsers

        [DynamicData(nameof(GetUserData), DynamicDataSourceType.Method)]
        [TestMethod()]
        public void GetUsers(UserModel expectedValue)
        {
            // Arrange
            _userRepositoryMock
               .Setup(x => x.GetAll())
               .Returns(CreateUserModels());

            // Act
            var result = _userService.GetUsers();

            // Assert
            Xunit.Assert.NotNull(result);
            Xunit.Assert.Equal(expectedValue.UserId, result.First().UserId);
            Xunit.Assert.Equal(expectedValue.Name, result.First().Name);
        }

        #endregion

        #region GetUserById

        [DynamicData(nameof(GetUserData), DynamicDataSourceType.Method)]
        [TestMethod()]
        public void GetUserById(UserModel expectedValue)
        {
            // Arrange
            var model = new UserModel() { UserId = 1, BirthDate = DateTime.UtcNow, IsActive = true, Name = "Clemente" };
            _userRepositoryMock.Setup(m => m.GetById(It.IsAny<int>()))
                              .Returns(Task.FromResult(model));

            // Act
            var result = _userService.GetUserById(1);

            // Assert
            Xunit.Assert.NotNull(result);
            Xunit.Assert.Equal(expectedValue.UserId, result.Result.UserId);
            Xunit.Assert.Equal(expectedValue.Name, result.Result.Name);
        }

        #endregion

        #region UserExists

        [TestMethod()]
        public void UserExists()
        {
            // Arrange
            _userRepositoryMock.Setup(m => m.Exists(It.IsAny<int>()))
                              .Returns(Task.FromResult(true));

            // Act
            var result = _userService.UserExists(1);

            // Assert
            Xunit.Assert.NotNull(result);
            Xunit.Assert.True(result.Result);
        }

        #endregion

        #region AddUser

        [TestMethod()]
        public void AddUser_WithNullUser_ThrowsAnException()
        {
            // Act
            Func<Task> sut = () => _userService.AddUser(null);

            // Assert
            sut.Should().ThrowAsync<ArgumentNullException>("user");
        }

        [TestMethod()]
        public void AddUser_WithInvalidUser_Returns_Failure()
        {
            // Arrange
            var model = new UserModel() { UserId = 1, BirthDate = DateTime.MinValue, IsActive = true, Name = "" };

            // Act
            var result = _userService.AddUser(model);

            // Assert
            Xunit.Assert.False(result.Result);
        }

        [TestMethod()]
        public void AddUser_WithValidUser_ThrowsAnException()
        {
            // Arrange
            var model = new UserModel() { UserId = 1, BirthDate = DateTime.UtcNow, IsActive = true, Name = "Clemente" };
            _userRepositoryMock.Setup(x => x.Insert(It.IsAny<UserModel>())).Throws(new Exception("Error"));

            // Act
            Func<Task> sut = () => _userService.AddUser(model);

            // Assert
            sut.Should().ThrowAsync<Exception>("Error");
            _userRepositoryMock.Verify(x => x.Save(), Times.Never);
        }

        [TestMethod()]
        public void AddUser_Successfully()
        {
            // Arrange
            var model = new UserModel() { UserId = 1, BirthDate = DateTime.UtcNow, IsActive = true, Name = "Clemente" };
            _userRepositoryMock.Setup(x => x.Insert(It.IsAny<UserModel>()));
            _userRepositoryMock.Setup(x => x.Save()).Returns(Task.FromResult(1));

            // Act
            var result = _userService.AddUser(model);

            // Assert
            Xunit.Assert.True(result.Result);
        }

        #endregion

        #region EditUser

        [TestMethod()]
        public void EditUser_WithInvalidUser_Returns_Failure()
        {
            // Arrange
            var model = new UserModel() { UserId = 1, BirthDate = DateTime.MinValue, IsActive = true, Name = "" };

            // Act
            var result = _userService.EditUser(model);

            // Assert
            Xunit.Assert.False(result.Result);
        }

        [TestMethod()]
        public void EditUser_WithValidUser_ThrowsAnException()
        {
            // Arrange
            var model = new UserModel() { UserId = 1, BirthDate = DateTime.UtcNow, IsActive = true, Name = "Clemente" };
            _userRepositoryMock.Setup(x => x.Update(It.IsAny<UserModel>())).Throws(new Exception("Error"));

            // Act
            Func<Task> sut = () => _userService.EditUser(model);

            // Assert
            sut.Should().ThrowAsync<Exception>("Error");
            _userRepositoryMock.Verify(x => x.Save(), Times.Never);
        }

        [TestMethod()]
        public void EditUser_Successfully()
        {
            // Arrange
            var model = new UserModel() { UserId = 1, BirthDate = DateTime.UtcNow, IsActive = true, Name = "Clemente" };
            _userRepositoryMock.Setup(x => x.Insert(It.IsAny<UserModel>()));
            _userRepositoryMock.Setup(x => x.Save()).Returns(Task.FromResult(1));

            // Act
            var result = _userService.EditUser(model);

            // Assert
            Xunit.Assert.True(result.Result);
        }

        #endregion

        #region RemoveUser

        [TestMethod()]
        public void RemoveUser_ThrowsAnException()
        {
            // Arrange
            _userRepositoryMock.Setup(x => x.Remove(It.IsAny<int>())).Throws(new Exception("Error"));

            // Act
            var result = _userService.DeleteUser(1);

            // Assert
            Xunit.Assert.False(result.Result);
            _userRepositoryMock.Verify(x => x.Save(), Times.Never);
        }

        [TestMethod()]
        public void RemoveUser_Successfully()
        {
            // Arrange
            _userRepositoryMock.Setup(x => x.Remove(It.IsAny<int>()));
            _userRepositoryMock.Setup(x => x.Save()).Returns(Task.FromResult(1));

            // Act
            var result = _userService.DeleteUser(1);

            // Assert
            Xunit.Assert.True(result.Result);
            _userRepositoryMock.Verify(x => x.Save(), Times.Once);
        }

        #endregion

        #region ActiveUsers

        [DynamicData(nameof(GetUserData), DynamicDataSourceType.Method)]
        [TestMethod()]
        public void GetActiveUsers(UserModel expectedValue)
        {
            // Arrange
            _userRepositoryMock
               .Setup(x => x.GetActiveUsers())
               .Returns(Task.FromResult(CreateUserModels()));

            // Act
            var result = _userService.GetActiveUsers();

            // Assert
            Xunit.Assert.NotNull(result);
            Xunit.Assert.Equal(expectedValue.UserId, result.Result.First().UserId);
            Xunit.Assert.Equal(expectedValue.Name, result.Result.First().Name);
        }

        #endregion

        #region Setup

        private UserService CreateUserService()
        {
            _userRepositoryMock = new Mock<IUserRepository>();

            return new UserService(_userRepositoryMock.Object);
        }

        private static List<UserModel> CreateUserModels()
        {
            var user = new UserModel
            {
                UserId = 1,
                Name = "Clemente",
                BirthDate = DateTime.UtcNow,
                IsActive = true
            };

            return new List<UserModel> { user };
        }

        #endregion

        #region MockData

        public static IEnumerable<object[]> GetUserData() => CreateUserModels().Select(ep => new[] { ep });

        #endregion
    }
}