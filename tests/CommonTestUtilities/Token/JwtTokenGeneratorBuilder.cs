using CashFlow.Domain.Entities;
using CashFlow.Domain.Security.Tokens;
using Moq;

namespace CommonTestUtilities.Token;

// Classe builder para criar um mock do gerador de tokens JWT
public class JwtTokenGeneratorBuilder
{
    public static IAccessTokenGenerator Build()
    {
        // Cria um mock da interface IAccessTokenGenerator
        var mock = new Mock<IAccessTokenGenerator>();

        // Configura o mock para retornar um token JWT fixo quando o método Generate for chamado
        mock.Setup(accessTokenGenerator => accessTokenGenerator
        .Generate(It.IsAny<User>()))
            .Returns("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c");

        return mock.Object;
    }
}