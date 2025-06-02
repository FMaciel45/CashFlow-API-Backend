namespace CashFlow.Application.UseCases.Expenses.Delete;
public interface IDeleteExpenseUseCase
{
    // Toda classe que implementar essa interface deve fornecer um método Execute que recebe um long id e retorna uma Task, sem lançar exceções não esperadas
    Task Execute(long id); // Task -> operação assíncrona
}