using Xunit;
using Restaurants.Domain.Constants;
using FluentAssertions;

namespace Restaurants.Application.Users.Tests;

public class CurrentUserTests
{
    //TestMethod_Scenario_ExpectedResult
    //[Fact()]
    [Theory()]
    [InlineData(UserRoles.Admin)]
    [InlineData(UserRoles.User)]
    public void IsInRole_WithMatchingRole_ShouldReturnTrue(string roleName)
    {
        //arrange
        var currentUser = new CurrentUser("1", "test@mail.com", [UserRoles.Admin,UserRoles.User],null,null);


        //act
        //var isInRole = currentUser.IsInRole(UserRoles.Admin);
        var isInRole = currentUser.IsInRole(roleName);

        //assert
        isInRole.Should().BeTrue();

    }

    [Fact()]
    public void IsInRole_WithNoMatchingRole_ShouldReturnFalse()
    {
        //arrange
        var currentUser = new CurrentUser("1", "test@mail.com", [UserRoles.Admin, UserRoles.User], null, null);


        //act
        var isInRole = currentUser.IsInRole(UserRoles.Owner);

        //assert
        isInRole.Should().BeFalse();

    }
}