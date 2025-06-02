using CashFlow.Communication.Requests;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.User;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using FluentValidation.Results;

namespace CashFlow.Application.UseCases.Users.Update;

// Implementação do caso de uso para atualização de dados do usuário
public class UpdateUserUseCase : IUpdateUserUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IUserUpdateOnlyRepository _repository; // Repositório especializado para atualização
    private readonly IUserReadOnlyRepository _userReadOnlyRepository; // Repositório para consultas
    private readonly IUnitOfWork _unitOfWork;

    // Construtor com injeção de dependências
    public UpdateUserUseCase(
        ILoggedUser loggedUser,
        IUserUpdateOnlyRepository repository,
        IUserReadOnlyRepository userReadOnlyRepository,
        IUnitOfWork unitOfWork)
    {
        _loggedUser = loggedUser;
        _repository = repository;
        _unitOfWork = unitOfWork;
        _userReadOnlyRepository = userReadOnlyRepository;
    }

    // Método principal que executa a atualização
    public async Task Execute(RequestUpdateUserJson request)
    {
        // Obtém o usuário autenticado
        var loggedUser = await _loggedUser.Get();

        // Valida os dados da requisição
        await Validate(request, loggedUser.Email);

        // Busca o usuário no BD pelo ID
        var user = await _repository.GetById(loggedUser.Id);

        // Atualiza os campos necessários
        user.Name = request.Name;
        user.Email = request.Email;

        // Marca a entidade como modificada
        _repository.Update(user);

        // Persiste as alterações no BD
        await _unitOfWork.Commit();
    }

    // Método privado para validação dos dados
    private async Task Validate(RequestUpdateUserJson request, string currentEmail)
    {
        // Validação com FluentValidation
        var validator = new UpdateUserValidator();
        var result = validator.Validate(request);

        // Verifica se email foi alterado e se já existe (validação adicional)
        if (currentEmail.Equals(request.Email) == false)
        {
            var userExist = await _userReadOnlyRepository.ExistActiveUserWithEmail(request.Email);
            if (userExist)
                result.Errors.Add(new ValidationFailure(string.Empty, ResourceErrorMessages.EMAIL_ALREADY_REGISTERED));
        }

        // Se houver erros, lança exceção
        if (result.IsValid == false)
        {
            var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMessages);
        }
    }
}