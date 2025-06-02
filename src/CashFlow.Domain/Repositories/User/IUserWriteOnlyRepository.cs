namespace CashFlow.Domain.Repositories.User;

// Repositório especializado em operações de escrita
public interface IUserWriteOnlyRepository
{
    Task Add(Entities.User user); // Adiciona um novo usuário ao sistema
    Task Delete(Entities.User user); // Remove um usuário existente
}