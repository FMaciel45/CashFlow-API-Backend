using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.User;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure.DataAccess.Repositories;

// Implementação concreta do repositório de usuários, responsável por todas as operações
internal class UserRepository : IUserReadOnlyRepository, IUserWriteOnlyRepository, IUserUpdateOnlyRepository
{
    private readonly CashFlowDbContext _dbContext; // Inicializa uma nova instância do repositório de usuários

    public UserRepository(CashFlowDbContext dbContext) => _dbContext = dbContext;

    public async Task Add(User user) // Adiciona um novo usuário ao contexto do banco de dados
    {
        await _dbContext.Users.AddAsync(user);
        // O SaveChanges (Commit) é delegado ao Unit of Work
    }

    public async Task Delete(User user) // Remove um usuário existente do banco de dados
    {
        var userToRemove = await _dbContext.Users.FindAsync(user.Id);
        _dbContext.Users.Remove(userToRemove!);
    }

    public async Task<bool> ExistActiveUserWithEmail(string email) // Verifica se existe um usuário ativo com o email especificado
    {
        return await _dbContext.Users.AnyAsync(user => user.Email.Equals(email));
    }

    public async Task<User> GetById(long id) // Obtém um usuário pelo ID 
    {
        // Com tracking para edição
        return await _dbContext.Users.FirstAsync(user => user.Id == id);
    }

    public async Task<User?> GetUserByEmail(string email) // Obtém um usuário pelo email
    {
        return await _dbContext.Users
            .AsNoTracking() // Sem tracking para + performance em operações readonly
            .FirstOrDefaultAsync(user => user.Email.Equals(email));
    }

    public void Update(User user) // Marca um usuário como modificado para atualização
    {
        _dbContext.Users.Update(user);
    }
}