using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Repositories.Expenses;

// Interface exclusiva para operações de criação/exclusão
public interface IExpensesWriteOnlyRepository
{
    Task Add(Expense expense);
    Task Delete(long id);
}