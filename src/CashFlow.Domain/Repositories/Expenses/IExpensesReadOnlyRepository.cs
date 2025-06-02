using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Repositories.Expenses;

// Interface específica para operações de leitura de despesas
public interface IExpensesReadOnlyRepository
{
    Task<List<Expense>> GetAll(Entities.User user); // Obtém todas as despesas de um usuário
    Task<Expense?> GetById(Entities.User user, long id); // Busca uma despesa específica por ID (? indica que pode retornar null se não encontrar)
    Task<List<Expense>> FilterByMonth(Entities.User user, DateOnly date); // Filtra despesas por mês/ano
}