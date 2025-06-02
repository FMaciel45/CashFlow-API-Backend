using CashFlow.Domain.Entities;
using CashFlow.Domain.Security.Tokens;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CashFlow.Infrastructure.Services.LoggedUser;

// Implementação de serviço para obtenção de usuário logado
internal class LoggedUser : ILoggedUser
{
    private readonly CashFlowDbContext _dbContext;
    private readonly ITokenProvider _tokenProvider;

    // Construtor que recebe as dependências necessárias via injeção de dependências
    public LoggedUser(CashFlowDbContext dbContext, ITokenProvider tokenProvider)
    {
        _dbContext = dbContext;
        _tokenProvider = tokenProvider;
    }

    // Obtém o usuário autenticado atual baseado no token JWT
    public async Task<User> Get()
    {
        // Extrai o token da requisição
        string token = _tokenProvider.TokenOnRequest();

        // Decodifica o token JWT sem validar (apenas leitura
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtSecurityToken = tokenHandler.ReadJwtToken(token);

        // Obtém o identificador único do usuário (claim Sid)
        var identifier = jwtSecurityToken.Claims.First(claim => claim.Type == ClaimTypes.Sid).Value;

        // Busca o usuário no BD
        return await _dbContext
            .Users
            .AsNoTracking() // + performance
            .FirstAsync(user => user.UserIdentifier == Guid.Parse(identifier));
    }
}