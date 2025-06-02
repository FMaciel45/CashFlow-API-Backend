namespace CashFlow.Communication.Responses;
public class ResponseRegisteredUserJson
{
    // DTO que representa a resposta após o registro bem sucedido de um usuário
    public string Name { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
}