using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.User;
using Moq;

namespace CommonTestUtilities.Repositories;

// Builder para a criação de mocks de IUserReadOnlyRepository
public class UserReadOnlyRepositoryBuilder
{
    private readonly Mock<IUserReadOnlyRepository> _repository;

    // Inicialização do builder criando um novo mock do repositório
    public UserReadOnlyRepositoryBuilder()
    {
        _repository = new Mock<IUserReadOnlyRepository>();
    }

    // Configuração do mock para retornar "true" ao verificar existência de email
    public void ExistActiveUserWithEmail(string email)
    {
        _repository
            .Setup(userReadOnly => userReadOnly
            .ExistActiveUserWithEmail(email))
            .ReturnsAsync(true);
    }

    // Configuração do mock para retornar um usuário específico ao buscar por email
    public UserReadOnlyRepositoryBuilder GetUserByEmail(User user)
    {
        _repository
            .Setup(userRepository => userRepository
            .GetUserByEmail(user.Email))
            .ReturnsAsync(user);

        return this;
    }

    // Finaliza a construção e retorna a instância mockada
    public IUserReadOnlyRepository Build() => _repository.Object;
}