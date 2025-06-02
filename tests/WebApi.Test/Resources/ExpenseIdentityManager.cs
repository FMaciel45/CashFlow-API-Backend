using CashFlow.Domain.Entities;

namespace WebApi.Test.Resources;

// Atua como um wrapper para a entidade Expense, expondo apenas propriedades específicas necessárias para testes
public class ExpenseIdentityManager
{
    // Campo privado readonly que armazena a instância da despesa
    private readonly Expense _expense;

    // Construtor que inicializa o gerenciador com uma despesa concreta
    public ExpenseIdentityManager(Expense expense)
    {
        _expense = expense;
    }

    // Obtém o ID da despesa encapsulada
    public long GetId() => _expense.Id;

    // Obtém a data da despesa encapsulada
    public DateTime GetDate() => _expense.Date;
}