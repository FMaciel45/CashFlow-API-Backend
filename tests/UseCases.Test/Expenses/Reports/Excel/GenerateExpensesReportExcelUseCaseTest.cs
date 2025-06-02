using CashFlow.Application.UseCases.Expenses.Reports.Excel;
using CashFlow.Domain.Entities;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using FluentAssertions;

namespace UseCases.Test.Expenses.Reports.Excel;
public class GenerateExpensesReportExcelUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        // Arrange 
        var loggedUser = UserBuilder.Build(); // Cria um usuário fictício para o teste
        var expenses = ExpenseBuilder.Collection(loggedUser); // Cria uma coleção de despesas associadas ao usuário

        // Cria a instância do caso de uso com as dependências mockadas
        var useCase = CreateUseCase(loggedUser, expenses);

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

        // Cria a instância do caso de uso com as dependências mockadas
        var useCase = CreateUseCase(loggedUser, new List<Expense>());

        // Act
        var result = await useCase.Execute(DateOnly.FromDateTime(DateTime.Today)); // Executa o caso de uso para a data atual

        // Assert
        result.Should().BeEmpty();
    }

    // Método auxiliar para criar a instância do caso de uso com dependências mockadas
    private GenerateExpensesReportExcelUseCase CreateUseCase(User user, List<Expense> expenses)
    {
        var repository = new ExpensesReadOnlyRepositoryBuilder().FilterByMonth(user, expenses).Build();
        var loggedUser = LoggedUserBuilder.Build(user);

        return new GenerateExpensesReportExcelUseCase(repository, loggedUser);
    }
}