using CashFlow.Domain.Repositories;
using Moq;

namespace CommonTestUtilities.Repositories;

// Cria um mock configurável de IUnitOfWork
public class UnitOfWorkBuilder
{
    public static IUnitOfWork Build()
    {
        var mock = new Mock<IUnitOfWork>();

        return mock.Object;
    }
}
