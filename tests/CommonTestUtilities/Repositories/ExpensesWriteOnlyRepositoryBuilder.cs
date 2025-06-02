using CashFlow.Domain.Repositories.Expenses;
using Moq;

namespace CommonTestUtilities.Repositories;

// Builder para a criação de mocks de IExpensesWriteOnlyRepository -> maneira rápida e padronizada de criar mocks para testes unitários
public class ExpensesWriteOnlyRepositoryBuilder
{
    // Cria e retorna um mock configurável do repositório de escrita de despesas
    public static IExpensesWriteOnlyRepository Build()
    {
        // Criação do mock
        var mock = new Mock<IExpensesWriteOnlyRepository>();

        // Retorna a interface mockada pronta para uso
        return mock.Object;
    }
}