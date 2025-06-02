using CashFlow.Domain.Enums;

namespace CashFlow.Domain.Entities;
public class Expense // É uma entidade de domínio, não uma DTO (por que é mapeada diretamente para a tabela Expenses no BD)
{
    // Classe que representa a entidade Despesa no domínio

    public long Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime Date { get; set; }
    public decimal Amount { get; set; }
    public PaymentType PaymentType { get; set; }
    public long UserId { get; set; }

    // Define o relacionamento com a tabela de usuário pra rodar as migrations sem ter que inserir User no DbContext (caso as tabelas não tivessem relação, seria necessário inserir User no DbContext antes de rodar as migrations)
    public User User { get; set; } = default!; // ! -> User nunca vai ser null
}