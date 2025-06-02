using CashFlow.Application.UseCases.Expenses.Reports.Pdf;
using CashFlow.Domain.Entities;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using FluentAssertions;

namespace UseCases.Test.Expenses.Reports.Pdf;
public class GenerateExpensesReportPdfUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        // Arrange
        var loggedUser = UserBuilder.Build(); // Cria um usuário fictício usando o UserBuilder
        var expenses = ExpenseBuilder.Collection(loggedUser); // Cria uma coleção de despesas associadas ao usuário

        var useCase = CreateUseCase(loggedUser, expenses); // Cria a instância do caso de uso com as dependências mockadas

        // Act
        var result = await useCase.Execute(DateOnly.FromDateTime(DateTime.Today)); // Executa o caso de uso para a data atual

        // Assert
        result.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task SuccessEmpty()
    {
        // Arrange
        var loggedUser = UserBuilder.Build();
        
        var useCase = CreateUseCase(loggedUser, []); // Cria o caso de uso com lista vazia de despesas

        // Act
        var result = await useCase.Execute(DateOnly.FromDateTime(DateTime.Today)); // Executa o caso de uso para a data atual

        // Assert
        result.Should().BeEmpty();
    }

    // Método auxiliar para criar a instância do caso de uso com dependências mockadas
    private GenerateExpensesReportPdfUseCase CreateUseCase(User user, List<Expense> expenses)
    {
        var repository = new ExpensesReadOnlyRepositoryBuilder().FilterByMonth(user, expenses).Build();
        var loggedUser = LoggedUserBuilder.Build(user);

        return new GenerateExpensesReportPdfUseCase(repository, loggedUser);
    }
}