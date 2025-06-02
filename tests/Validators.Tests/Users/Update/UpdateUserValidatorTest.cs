using CashFlow.Application.UseCases.Users.Update;
using CashFlow.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;

namespace Validators.Tests.Users.Update;
public class UpdateUserValidatorTest
{
    [Fact]
    public void Success()
    {
        // Arrange
        var validator = new UpdateUserValidator(); // Instancia o validador
        var request = RequestUpdateUserJsonBuilder.Build(); // Cria requisição válida

        // Act
        var result = validator.Validate(request); // Executa validação

        // Assert
        result.IsValid.Should().BeTrue(); // Verifica que é válido
    }

    [Theory]
    [InlineData("")]
    [InlineData("      ")]
    [InlineData(null)]
    public void ErrorNameEmpty(string name)
    {
        // Arrange
        var validator = new UpdateUserValidator();
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Name = name; // Define nome inválido (passado como parâmetro na função)

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.NAME_EMPTY));
    }

    [Theory]
    [InlineData("")]
    [InlineData("      ")]
    [InlineData(null)]
    public void ErrorEmailEmpty(string email)
    {
        // Arrange
        var validator = new UpdateUserValidator();
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Email = email; // Define email inválido (passado como parâmetro na função)

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.EMAIL_EMPTY));
    }

    [Fact]
    public void ErrorEmailInvalid()
    {
        // Arrange
        var validator = new UpdateUserValidator();
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Email = "felipe.com"; // Define email inválido (sem "@")

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.EMAIL_INVALID));
    }
}