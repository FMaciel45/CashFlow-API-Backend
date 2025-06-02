using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CashFlow.Communication.Requests;
public class RequestLoginJson
{
    // DTO (Data Transfer Object) com os dados necessários para autenticação de um usuário

    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}