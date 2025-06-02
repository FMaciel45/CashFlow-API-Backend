using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Expenses;
using Moq;

namespace CommonTestUtilities.Repositories;

// Builder para criar mocks de IExpensesReadOnlyRepository com comportamentos configuráveis que simulam os comportamentos reais da interface IExpensesReadOnlyRepository
public class ExpensesReadOnlyRepositoryBuilder
{
    // Armazena o mock do repositório de Readonly para despesas
    private readonly Mock<IExpensesReadOnlyRepository> _repository;

    // Inicializa o mock do repositório
    public ExpensesReadOnlyRepositoryBuilder()
    {
        _repository = new Mock<IExpensesReadOnlyRepository>();
    }

    // Configura o mock para retornar uma lista específica de despesas no GetAll
    public ExpensesReadOnlyRepositoryBuilder GetAll(User user, List<Expense> expenses)
    {
        _repository.Setup(repository => repository.GetAll(user)).ReturnsAsync(expenses);

        return this;
    }

    // Configura o mock para retornar uma despesa específica no GetById
    public ExpensesReadOnlyRepositoryBuilder GetById(User user, Expense? expense)
    {
        if (expense is not null)
            _repository.Setup(repository => repository.GetById(user, expense.Id)).ReturnsAsync(expense);

        return this;
    }

    // Configura o mock para retornar despesas filtradas por mês
    public ExpensesReadOnlyRepositoryBuilder FilterByMonth(User user, List<Expense> expenses)
    {
        _repository
            .Setup(repository => repository
            .FilterByMonth(user, It.IsAny<DateOnly>()))
            .ReturnsAsync(expenses);

        return this;
    }

    // Finaliza a construção do mock após todas as configurações, acessa a instância pronta através da propriedade Object e retorna a interface IExpensesReadOnlyRepository
    public IExpensesReadOnlyRepository Build() => _repository.Object;
}