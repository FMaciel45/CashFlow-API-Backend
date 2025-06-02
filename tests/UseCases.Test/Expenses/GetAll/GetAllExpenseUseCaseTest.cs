using CashFlow.Application.UseCases.Expenses.GetAll;
using CashFlow.Domain.Entities;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using FluentAssertions;

namespace UseCases.Test.Expenses.GetAll;
public class GetAllExpenseUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        // Arrange
        var loggedUser = UserBuilder.Build(); // Cria um usuário de teste
        var expenses = ExpenseBuilder.Collection(loggedUser); // Cria uma coleção de despesas de teste

        // Cria a instância do caso de uso com as dependências mockadas
        var useCase = CreateUseCase(loggedUser, expenses);

        // Act
        var result = await useCase.Execute(); // Executa o caso de uso

        // Assert
        result.Should().NotBeNull();
        result.Expenses.Should().NotBeNullOrEmpty().And.AllSatisfy(expense =>
        {
            expense.Id.Should().BeGreaterThan(0); 
            expense.Title.Should().NotBeNullOrEmpty();
            expense.Amount.Should().BeGreaterThan(0);
        });
    }

    private GetAllExpenseUseCase CreateUseCase(User user, List<Expense> expenses)
    {
        // Cria um repositório mockado com dados de teste
        var repository = new ExpensesReadOnlyRepositoryBuilder().GetAll(user, expenses).Build();
        var mapper = MapperBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);

        return new GetAllExpenseUseCase(repository, mapper, loggedUser);
    }
}