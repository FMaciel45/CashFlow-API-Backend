namespace CashFlow.Domain.Security.Tokens;

// Interface para extração de tokens de requisições HTTP
public interface ITokenProvider
{
    string TokenOnRequest(); // Obtém o token JWT da requisição atual
}