using CashFlow.Domain.Repositories;

namespace CashFlow.Infrastructure.DataAccess;

// Implementação concreta do padrão Unit of Work, responsável por gerenciar transações e persistir todas as alterações no banco de dados de uma só vez
internal class UnitOfWork : IUnitOfWork
{
    private readonly CashFlowDbContext _dbContext;

    // Construtor que recebe o contexto do banco de dados via injeção de dependência
    public UnitOfWork(CashFlowDbContext dbContext)
    {
        _dbContext = dbContext; // Inicializa o contexto
    }

    // Persiste todas as alterações pendentes no BD
    public async Task Commit() => await _dbContext.SaveChangesAsync();
}