using CashFlow.Domain.Enums;

namespace CashFlow.Domain.Entities;
public class User // É uma entidade de domínio, não uma DTO (por que é mapeada diretamente para a tabela Users no BD)
{
    public long Id { get; set; } // Identificador único no BD (chave primária)
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public Guid UserIdentifier { get; set; }
    public string Role { get; set; } = Roles.TEAM_MEMBER;
}