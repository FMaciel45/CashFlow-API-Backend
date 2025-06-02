using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Repositories.User;
using CashFlow.Domain.Security.Cryptography;
using CashFlow.Domain.Security.Tokens;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCases.Login.DoLogin;

// Implementação do caso de uso para autenticação de usuários
public class DoLoginUseCase : IDoLoginUseCase
{
    private readonly IUserReadOnlyRepository _repository; // Repositório de usuários
    private readonly IPasswordEncripter _passwordEncripter; // Serviço de criptografia/verificação de senhas
    private readonly IAccessTokenGenerator _accessTokenGenerator; // Gerador de tokens JWT

    // Construtor com injeção de dependências
    public DoLoginUseCase(
        IUserReadOnlyRepository repository,
        IPasswordEncripter passwordEncripter,
        IAccessTokenGenerator accessTokenGenerator)
    {
        _passwordEncripter = passwordEncripter;
        _repository = repository;
        _accessTokenGenerator = accessTokenGenerator;
    }

    // Método principal que executa o fluxo de login
    public async Task<ResponseRegisteredUserJson> Execute(RequestLoginJson request)
    {
        // Busca usuário pelo email
        var user = await _repository.GetUserByEmail(request.Email);

        // Se usuário existe
        if (user is null)
        { 
            throw new InvalidLoginException(); // Falha genérica por segurança (não revelar informações sensíveis)
        }

        // Verifica se a senha está correta
        var passwordMatch = _passwordEncripter.Verify(request.Password, user.Password);

        // Caso a senha não esteja correta
        if(passwordMatch == false)
        {
            throw new InvalidLoginException(); // Exceção genérica (+ segurança)
        }

        // Retorna resposta com token JWT
        return new ResponseRegisteredUserJson
        {
            Name = user.Name,
            Token = _accessTokenGenerator.Generate(user)
        };
    }
}