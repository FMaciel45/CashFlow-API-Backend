namespace CashFlow.Communication.Requests;
public class RequestUpdateUserJson
{
    // DTO (Data Transfer Object) que possui os dados necessários para atualizar informações de um usuário 

    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}