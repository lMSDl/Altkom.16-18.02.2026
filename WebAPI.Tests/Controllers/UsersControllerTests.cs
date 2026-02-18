using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebAPI.Controllers;
using WebAPI.Models;
using WebAPI.Services;

namespace WebAPI.Tests.Controllers
{
    public class UsersControllerTests
    {

        [Fact]
        public async Task Get_OkWithAllUsers()
        {
            //Arrage
            var userService = new Mock<ICrudService<User>>();
            var controller = new UsersController(userService.Object);

            var users = new Fixture().CreateMany<User>();
            userService.Setup(x => x.ReadAllAsync()).ReturnsAsync(users);

            //Act
            var result = await controller.Get();

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var resultUsers = Assert.IsAssignableFrom<IEnumerable<User>>(okResult.Value);
            Assert.Equal(users, resultUsers);
        }

        [Fact]
        public async Task Get_ExistingId_OkWithUser()
        {
            //Arrage
            var userService = new Mock<ICrudService<User>>();
            var controller = new UsersController(userService.Object);

            var user = new Fixture().Create<User>();
            userService.Setup(x => x.ReadAsync(user.Id)).ReturnsAsync(user);

            //Act
            var result = await controller.Get(user.Id);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var resultUser = Assert.IsAssignableFrom<User>(okResult.Value);
            Assert.Equal(user, resultUser);
        }

        [Fact]
        public Task Get_NotExistingId_NotFound()
        {
            return ReturnsNotFound(async (controller, id) => (await controller.Get(id)).Result!);
        }

        [Fact]
        public Task Put_NotExistingId_NotFound()
        {
            return ReturnsNotFound((controller, id) => controller.Put(id, default!));
        }

        [Fact]
        public Task Delete_NotExistingId_NotFound()
        {
            return ReturnsNotFound((controller, id) => controller.Delete(id));
        }


        private async Task ReturnsNotFound(Func<UsersController, int, Task<ActionResult>> func)
        {
            //Arrage
            var userService = new Mock<ICrudService<User>>();
            var controller = new UsersController(userService.Object);

            //Act
            var result = await func(controller, default);

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
