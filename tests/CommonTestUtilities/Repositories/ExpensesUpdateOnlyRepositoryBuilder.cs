using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Expenses;
using Moq;

namespace CommonTestUtilities.Repositories;

// Cria mocks configuráveis para testar cenários de atualização de despesas
public class ExpensesUpdateOnlyRepositoryBuilder
{
    private readonly Mock<IExpensesUpdateOnlyRepository> _repository;

    // Inicializa o mock
    public ExpensesUpdateOnlyRepositoryBuilder()
    {
        _repository = new Mock<IExpensesUpdateOnlyRepository>();
    }

    // Configura o comportamento do GetById
    public ExpensesUpdateOnlyRepositoryBuilder GetById(User user, Expense? expense)
    {
        if (expense is not null)
            _repository
                .Setup(repository => repository
                .GetById(user, expense.Id))
                .ReturnsAsync(expense); // Retorna a despesa quando encontrada

        return this; // Method chaining -> pesquisar
    }

    // Finaliza a construção do mock
    public IExpensesUpdateOnlyRepository Build() => _repository.Object;
}