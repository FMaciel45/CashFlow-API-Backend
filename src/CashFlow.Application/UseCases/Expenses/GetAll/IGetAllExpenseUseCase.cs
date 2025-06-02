
using CashFlow.Communication.Responses;

namespace CashFlow.Application.UseCases.Expenses.GetAll;
public interface IGetAllExpenseUseCase
{
    // Método assíncrono que retorna a lista de despesas
    Task<ResponseExpensesJson> Execute(); // <> -> define o retorno
}