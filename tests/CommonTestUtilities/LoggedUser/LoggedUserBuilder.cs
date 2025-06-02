using CashFlow.Domain.Entities;
using CashFlow.Domain.Services.LoggedUser;
using Moq;

namespace CommonTestUtilities.LoggedUser;

// Builder para criar um mock de ILoggedUser para testes
public class LoggedUserBuilder
{
    // Cria um mock configurado para retornar o usuário especificado
    public static ILoggedUser Build(User user)
    {
        // Cria um mock da interface ILoggedUser 
        var mock = new Mock<ILoggedUser>();

        // Configura o mock para retornar o usuário fornecido
        mock.Setup(loggedUser => loggedUser.Get()).ReturnsAsync(user);

        // Retorna a instância mockada
        return mock.Object;
    }
}