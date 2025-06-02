using CashFlow.Application.UseCases.Users.ChangePassword;
using CashFlow.Domain.Entities;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FluentAssertions;

namespace UseCases.Test.Users.ChangePassword;
public class ChangePasswordUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        // Arrange
        var user = UserBuilder.Build();

        var request = RequestChangePasswordJsonBuilder.Build();

        var useCase = CreateUseCase(user, request.Password); // Configura caso de uso

        // Act
        var act = async () => await useCase.Execute(request); // Executa alteração de senha 

        // Assert
        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task ErrorNewPasswordEmpty()
    {
        // Arrange
        var user = UserBuilder.Build();

        var request = RequestChangePasswordJsonBuilder.Build();
        request.NewPassword = string.Empty; // Força senha inválida

        var useCase = CreateUseCase(user, request.Password);

        // Act
        var act = async () => { await useCase.Execute(request); };

        // Assert
        var result = await act.Should().ThrowAsync<ErrorOnValidationException>();
        
        result.Where(e => e.GetErrors().Count == 1 &&
                e.GetErrors().Contains(ResourceErrorMessages.INVALID_PASSWORD));
    }

    [Fact]
    public async Task ErrorCurrentPasswordDifferent()
    {
        // Arrange
        var user = UserBuilder.Build();

        var request = RequestChangePasswordJsonBuilder.Build();

        var useCase = CreateUseCase(user); // Não configura senha

        // Act
        var act = async () => { await useCase.Execute(request); };

        // Assert
        var result = await act.Should().ThrowAsync<ErrorOnValidationException>(); // Verifica exceção
        result.Where(e => e.GetErrors().Count == 1 &&
                e.GetErrors().Contains(ResourceErrorMessages.PASSWORD_DIFFERENT_CURRENT_PASSWORD));
    }

    // Método auxiliar para criar a instância do caso de uso
    private static ChangePasswordUseCase CreateUseCase(User user, string? password = null)
    {
        var unitOfWork = UnitOfWorkBuilder.Build();
        var userUpdateRepository = UserUpdateOnlyRepositoryBuilder.Build(user);
        var loggedUser = LoggedUserBuilder.Build(user);
        var passwordEncripter = new PasswordEncrypterBuilder().Verify(password).Build();

        return new ChangePasswordUseCase(loggedUser, passwordEncripter, userUpdateRepository, unitOfWork);
    }
}