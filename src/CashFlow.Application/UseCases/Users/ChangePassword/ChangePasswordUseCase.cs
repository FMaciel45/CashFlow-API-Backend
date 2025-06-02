using CashFlow.Communication.Requests;
using CashFlow.Domain.Repositories.User;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Security.Cryptography;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Exception.ExceptionsBase;
using FluentValidation.Results;
using CashFlow.Exception;

namespace CashFlow.Application.UseCases.Users.ChangePassword;

// Caso de uso para mudar a senha do usuário
public class ChangePasswordUseCase : IChangePasswordUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IUserUpdateOnlyRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordEncripter _passwordEncripter;

    public ChangePasswordUseCase(
        ILoggedUser loggedUser,
        IPasswordEncripter passwordEncripter,
        IUserUpdateOnlyRepository repository,
        IUnitOfWork unitOfWork)
    {
        _loggedUser = loggedUser;
        _repository = repository;
        _unitOfWork = unitOfWork;
        _passwordEncripter = passwordEncripter;
    }

    // Método que executa a injeção de dependências
    public async Task Execute(RequestChangePasswordJson request)
    {
        // Obtém o usuário logado
        var loggedUser = await _loggedUser.Get();

        // Valida a requisição
        Validate(request, loggedUser);

        // Busca o usuário no banco de dados
        var user = await _repository.GetById(loggedUser.Id);

        // Criptografa e atualiza a nova senha
        user.Password = _passwordEncripter.Encrypt(request.NewPassword);

        // Marca o usuário como modificado
        _repository.Update(user);

        // Persiste as alterações no BD
        await _unitOfWork.Commit();
    }

    // Método privado para validação dos dados
    private void Validate(RequestChangePasswordJson request, Domain.Entities.User loggedUser)
    {
        // Validação básica com FluentValidation
        var validator = new ChangePasswordValidator();
        var result = validator.Validate(request);

        // Confere se a senha atual está correta
        var passwordMatch = _passwordEncripter.Verify(request.Password, loggedUser.Password);

        if (passwordMatch == false)
        {
            result.Errors.Add(new ValidationFailure(string.Empty, ResourceErrorMessages.PASSWORD_DIFFERENT_CURRENT_PASSWORD));
        }

        // Se houver erros, lança exceção
        if (result.IsValid == false)
        {
            var errors = result.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errors);
        }
    }
}