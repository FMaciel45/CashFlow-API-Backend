using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Repositories.Expenses;

// Interface especializada em operações de atualização
public interface IExpensesUpdateOnlyRepository
{
    Task<Expense?> GetById(Entities.User user, long id); // Obtém uma despesa específica para edição (? indica que pode retornar null se não existir)
    void Update(Expense expense); // Marca uma entidade Expense como modificada para atualização
}