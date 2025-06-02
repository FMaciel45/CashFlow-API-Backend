using CashFlow.Domain.Security.Tokens;

namespace CashFlow.Api.Token;

public class HttpContextTokenValue : ITokenProvider // ITokenProvider -> obtém o token JWT do HttpContext
{
    // Dependência que fornece acesso ao HttpContext atual
    private readonly IHttpContextAccessor _contextAccessor;

    // Construtor que recebe a dependência IHttpContextAccessor por injeção
    public HttpContextTokenValue(IHttpContextAccessor httpContextAccessor)
    {
        _contextAccessor = httpContextAccessor;
    }

    // Método que extrai o token JWT do cabeçalho Authorization da requisição
    public string TokenOnRequest()
    {
        // Obtém o valor completo do cabeçalho Authorization
        var authorization = _contextAccessor.HttpContext!.Request.Headers.Authorization.ToString();

        // Remove o prefixo "Bearer " da requisição e retorna apenas o token
        return authorization["Bearer ".Length..].Trim();
    }
}