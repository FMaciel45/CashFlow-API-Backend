namespace CashFlow.Communication.Responses;
public class ResponseErrorJson
{
    // DTO (Data Transfer Object) para respostas de erro padronizadas da API

    public List<string> ErrorMessages { get; set; } // Lista para mensagens de erro específicas

    public ResponseErrorJson(string errorMessage) // Construtor para erro único
    {
        ErrorMessages = [errorMessage]; // Coleção simplificada
    }

    public ResponseErrorJson(List<string> errorMessage) // Construtor para vários erros
    {
        ErrorMessages = errorMessage;
    }
}