using Restaurants.Application.Users;
using Xunit;
using Moq;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Restaurants.Domain.Constants;
using FluentAssertions;

namespace Restaurants.Application.Users.Tests;

public class UserContextTests
{
    [Fact()]
    public void GetCurrentUserTest_AuthenticatedUser_ShouldReturnCurrentUser()
    {
        //arrange
        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        var dateOfBirth = new DateOnly(1990, 1, 1);
        var claims = new List<Claim>()
        {
            new (ClaimTypes.NameIdentifier,"1"),
            new (ClaimTypes.Email,"test@mail.com"),
            new (ClaimTypes.Role,UserRoles.Admin),
            new (ClaimTypes.Role,UserRoles.User),
            new ("Nationality","German"),
            new ("DateOfBirth",dateOfBirth.ToString("yyyy-MM-dd"))
        };

        var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "Test"));

        httpContextAccessorMock.Setup(x => x.HttpContext).Returns(new DefaultHttpContext()
        {
            User = user
        });

        var userContext = new UserContext(httpContextAccessorMock.Object);


        //act
        var currentuser = userContext.GetCurrentUser();

        //assert
        currentuser.Should().NotBeNull();
        currentuser.Id.Should().Be("1");
        currentuser.Email.Should().Be("test@mail.com");
        currentuser.Roles.Should().ContainInOrder(UserRoles.Admin, UserRoles.User);
        currentuser.Nationality.Should().Be("German");
        currentuser.DateOfBirth.Should().Be(dateOfBirth);
    }

    [Fact()]
    public void GetCurrentUserTest_WithNullUserContext_ThrowInvalidOperationException()
    {
        //arrange
        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        httpContextAccessorMock.Setup(x => x.HttpContext).Returns((HttpContext)null);

        var userContext = new UserContext(httpContextAccessorMock.Object);
        
        //act
        Action action = () => userContext.GetCurrentUser();

        //assert
        action.Should().Throw<InvalidOperationException>()
            .WithMessage("User context is not present");

    }
}