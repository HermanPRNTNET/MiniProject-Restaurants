using Xunit;
using FluentValidation.TestHelper;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant.Tests;

public class CreateRestaurantCommandValidatorTests
{
    [Fact()]
    public void Validator_ForValidCommand_ShouldNotHaveValidationErrors()
    {
        //arrange
        var command = new CreateRestaurantCommand
        {
            Name = "Test Restaurant",
            Description = "Test Description",
            Category = "Italian",
            ContactEmail = "test@mail.com",
            PostalCode = "12-345"
        };

        var validator = new CreateRestaurantCommandValidator();

        //act
        var result = validator.TestValidate(command);


        //assert

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact()]
    public void Validator_ForInvalidCommand_ShouldHaveValidationErrors()
    {
        //arrange
        var command = new CreateRestaurantCommand
        {
            Name = "Te",
            Description = "Te",
            Category = "Indo",
            ContactEmail = "@mail.com",
            PostalCode = "12345"
        };

        var validator = new CreateRestaurantCommandValidator();

        //act
        var result = validator.TestValidate(command);


        //assert

        result.ShouldHaveValidationErrorFor(x => x.Name);
        result.ShouldHaveValidationErrorFor(x => x.Category);
        result.ShouldHaveValidationErrorFor(x => x.ContactEmail);
        result.ShouldHaveValidationErrorFor(x => x.PostalCode);
    }

    [Theory()]
    [InlineData("Italian")]
    [InlineData("Mexican")]
    [InlineData("Japanese")]
    [InlineData("America")]
    [InlineData("Indian")]
    public void Validator_ForValidCategory_ShouldNotHaveValidationErrors(string category)
    {
        //arrange
        var validator = new CreateRestaurantCommandValidator();
        var command = new CreateRestaurantCommand { Category = category };
        //act
        var result = validator.TestValidate(command);


        //assert

        result.ShouldNotHaveValidationErrorFor(c => category);
    }

    [Theory()]
    [InlineData("1120")]
    [InlineData("102-00")]
    [InlineData("10 10")]
    [InlineData("10-2 20")]
    public void Validator_ForValidPostalCode_ShouldHaveValidationErrorsForPostalCode(string postalCode)
    {
        //arrange
        var validator = new CreateRestaurantCommandValidator();
        var command = new CreateRestaurantCommand { PostalCode = postalCode };
        //act
        var result = validator.TestValidate(command);


        //assert

        result.ShouldNotHaveValidationErrorFor(c => postalCode);
    }
}