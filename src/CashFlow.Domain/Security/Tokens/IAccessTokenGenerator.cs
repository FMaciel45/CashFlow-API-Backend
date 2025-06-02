using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Security.Tokens;

// Interface para abstração da geração de tokens JWT
public interface IAccessTokenGenerator
{
    string Generate(User user); // Gera um token de acesso para o usuário especificado

}