using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.User;
using CashFlow.Domain.Services.LoggedUser;

namespace CashFlow.Application.UseCases.Users.Delete;

// Implementação do caso de uso para exclusão de conta de usuário
public class DeleteUserAccountUseCase : IDeleteUserAccountUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IUserWriteOnlyRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteUserAccountUseCase(
        IUnitOfWork unitOfWork,
        IUserWriteOnlyRepository repository,
        ILoggedUser loggedUser)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _loggedUser = loggedUser;
    }

    public async Task Execute()
    {
        // Obtém o usuário atualmente autenticado
        var user = await _loggedUser.Get();

        // Executa a exclusão no repositório
        await _repository.Delete(user);

        // Confirma / Persiste a transação no banco de dados
        await _unitOfWork.Commit();
    }
}