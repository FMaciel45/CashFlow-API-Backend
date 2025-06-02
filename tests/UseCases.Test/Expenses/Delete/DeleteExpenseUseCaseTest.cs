using CashFlow.Application.UseCases.Expenses.Delete;
using CashFlow.Domain.Entities;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using FluentAssertions; // Pacote que facilita a escrita de assertions (verificações) em testes unitários

namespace UseCases.Test.Expenses.Delete;

// AAA (Arrange-Act-Assert)
// Teste de unidade que garante o comportamento básico do caso de uso de exclusão de despesa
public class DeleteExpenseUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        // Arrange
        var loggedUser = UserBuilder.Build(); // Cria usuário fictício
        var expense = ExpenseBuilder.Build(loggedUser); // Cria despesa associada ao usuário

        // Cria a instância do caso de uso com as dependências mockadas
        var useCase = CreateUseCase(loggedUser, expense);

        // Act
        var act = async () => await useCase.Execute(expense.Id); // Executa o caso de uso abaixo

        // Assert com o Fluent Assertions
        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task ErrorExpenseNotFound()
    {
        // Arrange
        var loggedUser = UserBuilder.Build();

        // Cria a instância do caso de uso com as dependências mockadas
        var useCase = CreateUseCase(loggedUser); // Sem passar expense como parâmetro p/ dar o erro

        // Act -> executa o caso de uso
        var act = async () => await useCase.Execute(id: 1000); // ID inexistente

        // Assert
        var result = await act.Should().ThrowAsync<NotFoundException>();
        result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.EXPENSE_NOT_FOUND));
    }

    private DeleteExpenseUseCase CreateUseCase(User user, Expense? expense = null)
    {
        // Cria os repositórios mockados
        var repositoryWriteOnly = ExpensesWriteOnlyRepositoryBuilder.Build();
        var repository = new ExpensesReadOnlyRepositoryBuilder().GetById(user, expense).Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);

        return new DeleteExpenseUseCase(repositoryWriteOnly, repository, unitOfWork, loggedUser);
    }
}