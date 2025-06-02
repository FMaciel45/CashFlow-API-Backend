namespace CashFlow.Domain.Repositories.User;

// Repositório especializado em consultas de usuários
public interface IUserReadOnlyRepository
{
    Task<bool> ExistActiveUserWithEmail(string email); // Verifica se existe um usuário ativo com o email fornecido
    Task<Entities.User?> GetUserByEmail(string email); // Obtém um usuário completo pelo email (? indica que pode retornar null se não existir)
}