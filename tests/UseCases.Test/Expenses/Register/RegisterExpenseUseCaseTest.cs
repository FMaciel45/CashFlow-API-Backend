using CashFlow.Application.UseCases.Expenses.Register;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FluentAssertions;

namespace UseCases.Test.Expenses.Register;
public class RegisterExpenseUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        // Arrange
        var loggedUser = UserBuilder.Build();
        var request = RequestExpenseJsonBuilder.Build();
        
        var useCase = CreateUseCase(loggedUser); // Instância do caso de uso

        // Act -> executa o caso de uso
        var result = await useCase.Execute(request);

        // Assert
        result.Should().NotBeNull();
        result.Title.Should().Be(request.Title); // Verifica se o título foi mantido
    }

    [Fact]
    public async Task ErrorTitleEmpty()
    {
        // Arrange
        var loggedUser = UserBuilder.Build();
        var request = RequestExpenseJsonBuilder.Build();

        request.Title = string.Empty; // Força erro de validação

        var useCase = CreateUseCase(loggedUser);

        // Act
        var act = async () => await useCase.Execute(request);

        // Assert
        var result = await act.Should().ThrowAsync<ErrorOnValidationException>();
        result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.TITLE_REQUIRED));
    }

    // Centraliza a criação do caso de uso
    private RegisterExpenseUseCase CreateUseCase(CashFlow.Domain.Entities.User user)
    {
        var repository = ExpensesWriteOnlyRepositoryBuilder.Build();
        var mapper = MapperBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);

        return new RegisterExpenseUseCase(repository, unitOfWork, mapper, loggedUser);
    }
}