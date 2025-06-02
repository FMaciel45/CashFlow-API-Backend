namespace CashFlow.Communication.Requests;
public class RequestRegisterUserJson
{
    // DTO (Data Transfer Object) que possui os dados necessários para registrar um novo usuário 

    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}