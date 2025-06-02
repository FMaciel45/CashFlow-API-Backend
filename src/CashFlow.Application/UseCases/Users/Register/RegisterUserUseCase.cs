using AutoMapper;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.User;
using CashFlow.Domain.Security.Cryptography;
using CashFlow.Domain.Security.Tokens;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using FluentValidation.Results;

namespace CashFlow.Application.UseCases.Users.Register;

// Implementação do caso de uso para registro de novos usuários
public class RegisterUserUseCase : IRegisterUserUseCase
{
    private readonly IMapper _mapper;
    private readonly IPasswordEncripter _passwordEncripter;
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;
    private readonly IUserWriteOnlyRepository _userWriteOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAccessTokenGenerator _tokenGenerator;

    // Construtor com injeção de dependências
    public RegisterUserUseCase(
        IMapper mapper,
        IPasswordEncripter passwordEncripter,
        IUserReadOnlyRepository userReadOnlyRepository,
        IUserWriteOnlyRepository userWriteOnlyRepository,
        IAccessTokenGenerator tokenGenerator,
        IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _passwordEncripter = passwordEncripter;
        _userReadOnlyRepository = userReadOnlyRepository;
        _userWriteOnlyRepository = userWriteOnlyRepository;
        _unitOfWork = unitOfWork;
        _tokenGenerator = tokenGenerator;
    }

    // Método principal que executa o registro
    public async Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserJson request)
    {
        // Valida os dados de entrada
        await Validate(request);

        // Mapeia o DTO para a entidade User
        var user = _mapper.Map<Domain.Entities.User>(request);

        // Criptografa a senha ANTES de armazenar
        user.Password = _passwordEncripter.Encrypt(request.Password);

        // Gera um identificador único para o usuário
        user.UserIdentifier = Guid.NewGuid();

        // Adiciona o novo usuário
        await _userWriteOnlyRepository.Add(user);

        // Persiste a transação no BD
        await _unitOfWork.Commit();

        // Retorna resposta o nome do usuário criado e o seu token JWT
        return new ResponseRegisteredUserJson
        {
            Name = user.Name,
            Token = _tokenGenerator.Generate(user)
        };
    }

    // Método para validação dos dados
    private async Task Validate(RequestRegisterUserJson request)
    {
        // Validação básica com FluentValidation
        var result = new RegisterUserValidator().Validate(request);

        // Verifica se o email já é cadastrado no BD 
        var emailExist = await _userReadOnlyRepository.ExistActiveUserWithEmail(request.Email);

        // Se o email já for registrado, erro
        if(emailExist)
        {
            result.Errors.Add(new ValidationFailure(string.Empty, ResourceErrorMessages.EMAIL_ALREADY_REGISTERED));
        }

        // Se houver erros na operação, lança uma exceção
        if (result.IsValid == false)
        {
            var errorMessages = result.Errors.Select(f => f.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMessages);
        }
    }
}