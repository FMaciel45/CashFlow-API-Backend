namespace CashFlow.Communication.Responses;
public class ResponseExpensesJson
{
    // DTO para respostas agrupadas de despesas

    public List<ResponseShortExpenseJson> Expenses { get; set; } = [];
}