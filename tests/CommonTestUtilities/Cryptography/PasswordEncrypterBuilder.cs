using CashFlow.Domain.Security.Cryptography;
using Moq; // Permite criar simulações para testar o comportamento de partes do código

namespace CommonTestUtilities.Cryptography;

// Classe para criar mocks de IPasswordEncripter de forma flexível durante testes
public class PasswordEncrypterBuilder
{
    private readonly Mock<IPasswordEncripter> _mock;

    public PasswordEncrypterBuilder()
    {
        // Inicializa o mock com comportamentos padrão
        _mock = new Mock<IPasswordEncripter>();

        // Encrypt sempre retorna "!%dlfjkd545" para facilitar os testes
        _mock.Setup(passwordEncrypter => passwordEncrypter.Encrypt(It.IsAny<string>())).Returns("!%dlfjkd545");
    }

    // Configura o mock para retornar true ao verificar uma senha específica
    public PasswordEncrypterBuilder Verify(string? password)
    {
        if(string.IsNullOrWhiteSpace(password) == false)
        {
            _mock.Setup(passwordEncrypter => passwordEncrypter.Verify(password, It.IsAny<string>())).Returns(true); // Configuração da verificação para uma senha específica
        }
        
        return this; // Method chaining (encadeamento de métodos) -> configura múltiplos comportamentos em uma única linha
    }

    // Retorna uma instância do mock pronta para uso nos testes
    public IPasswordEncripter Build() => _mock.Object;
}