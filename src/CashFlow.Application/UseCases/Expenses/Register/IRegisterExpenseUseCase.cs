using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;

namespace CashFlow.Application.UseCases.Expenses.Register;
public interface IRegisterExpenseUseCase
{
    // Recebe um RequestExpenseJson e retorna um ResponseRegisteredExpenseJson
    Task<ResponseRegisteredExpenseJson> Execute(RequestExpenseJson request);
}