using CashFlow.Communication.Enums;

namespace CashFlow.Communication.Responses;
public class ResponseExpenseJson
{
    // DTO para respostas detalhadas de despesas

    public long Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; } // ? -> indica que a propriedade pode ser nula
    public DateTime Date { get; set; }
    public decimal Amount { get; set; }
    public PaymentType PaymentType { get; set; }
}
