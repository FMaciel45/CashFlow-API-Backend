namespace CashFlow.Domain.Repositories.User;

// Repositório especializado em operações de atualização de usuários
public interface IUserUpdateOnlyRepository
{
    Task<Entities.User> GetById(long id); // Obtém um usuário pelo ID para edição
    void Update(Entities.User user); // Marca um usuário como modificado para atualização
}