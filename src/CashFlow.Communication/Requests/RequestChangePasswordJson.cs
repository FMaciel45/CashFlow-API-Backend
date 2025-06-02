using System.Runtime.Intrinsics.X86;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CashFlow.Communication.Requests;
public class RequestChangePasswordJson
{
    // DTO (Data Transfer Object) com a estrutura de dados necessária para alteração de senha

    public string Password { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}