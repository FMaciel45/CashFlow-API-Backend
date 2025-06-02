using CashFlow.Domain.Entities;

namespace WebApi.Test.Resources;

// Classe gerenciadora de identidade para usuários em contextos de teste
public class UserIdentityManager
{
    private readonly User _user;
    private readonly string _password;
    private readonly string _token;

    // Construtor que inicializa o gerenciador com todos os componentes de identidade do usuário
    public UserIdentityManager(User user, string password, string token)
    {
        _user = user;
        _password = password;
        _token = token;
    }

    // Obtém o nome, email, senha (desencriptada) e token do usuário encapsulado
    public string GetName() => _user.Name;
    public string GetEmail() => _user.Email;
    public string GetPassword() => _password;
    public string GetToken() => _token;
}