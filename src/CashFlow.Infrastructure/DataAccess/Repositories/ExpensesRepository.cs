using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Expenses;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure.DataAccess.Repositories;

// Implementação concreta do repositório de despesas, responsável por todas as operações de CRUD
// Modificador internal -> só é visível dentro do projeto CashFlow.Infrastructure
internal class ExpensesRepository : IExpensesReadOnlyRepository, IExpensesWriteOnlyRepository, IExpensesUpdateOnlyRepository
{
    private readonly CashFlowDbContext _dbContext;
    public ExpensesRepository(CashFlowDbContext dbContext) // Inicializa uma nova instância do repositório de despesas
    {
        _dbContext = dbContext;
    }

    public async Task Add(Expense expense) // Adiciona uma nova despesa ao contexto do banco de dados
    {
        await _dbContext.Expenses.AddAsync(expense);
        // O SaveChanges (Commit) é intencionalmente deixado para o Unit of Work 
    }

    public async Task Delete(long id) // Remove uma despesa do BD pelo ID
    {
        var result = await _dbContext.Expenses.FindAsync(id);

        _dbContext.Expenses.Remove(result!);
    }

    public async Task<List<Expense>> GetAll(User user) // Obtém todas as despesas de um usuário específico
    {
        return await _dbContext.Expenses
            .AsNoTracking() // Melhora performance para operações somente leitura
            .Where(expense => expense.UserId == user.Id)
            .ToListAsync();
    }

    async Task<Expense?> IExpensesReadOnlyRepository.GetById(User user, long id) // Obtém uma despesa específica por ID para operações de leitura
    {
        return await _dbContext.Expenses
            .AsNoTracking()
            .FirstOrDefaultAsync(expense => expense.Id == id && expense.UserId == user.Id);
    }

    async Task<Expense?> IExpensesUpdateOnlyRepository.GetById(User user, long id) // Obtém uma despesa específica por ID para operações de atualização
    {
        return await _dbContext.Expenses
            .FirstOrDefaultAsync(expense => expense.Id == id && expense.UserId == user.Id); 
    }

    public void Update(Expense expense) // Marca uma despesa como modificada para atualização
    {
        _dbContext.Expenses.Update(expense);
    }

    // Filtra despesas por mês/ano para um usuário específico
    public async Task<List<Expense>> FilterByMonth(User user, DateOnly date)
    {
        var startDate = new DateTime(year: date.Year, month: date.Month, day: 1).Date;

        var daysInMonth = DateTime.DaysInMonth(year: date.Year, month: date.Month);
        var endDate = new DateTime(year: date.Year, month: date.Month, day: daysInMonth, hour: 23, minute: 59, second: 59);

        return await _dbContext
            .Expenses
            .AsNoTracking()
            .Where(expense => expense.UserId == user.Id && expense.Date >= startDate && expense.Date <= endDate)
            .OrderBy(expense => expense.Date)
            .ThenBy(expense => expense.Title)
            .ToListAsync();
    }
}