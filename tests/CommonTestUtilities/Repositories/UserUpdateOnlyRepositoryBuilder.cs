using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.User;
using Moq;

namespace CommonTestUtilities.Repositories;

// Builder para criação de mocks de IUserUpdateOnlyRepository
public class UserUpdateOnlyRepositoryBuilder
{
    // Cria um mock configurado para retornar um usuário específico ao buscar por ID
    public static IUserUpdateOnlyRepository Build(User user)
    {
        // Criação do mock
        var mock = new Mock<IUserUpdateOnlyRepository>();

        mock // Configuração do método GetById para retornar o usuário fornecido
            .Setup(repository => repository
            .GetById(user.Id))
            .ReturnsAsync(user);

        return mock.Object;
    }
}