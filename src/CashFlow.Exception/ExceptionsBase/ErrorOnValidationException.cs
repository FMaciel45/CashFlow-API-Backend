using System.Net;

namespace CashFlow.Exception.ExceptionsBase;

// Exceção especializada para erros de validação na aplicação
public class ErrorOnValidationException : CashFlowException
{
    private readonly List<string> _errors; // Lista interna readonly (imutável) para armazenar os erros de validação

    public override int StatusCode => (int)HttpStatusCode.BadRequest; // Define o código HTTP de resposta padrão como 400 (Bad Request)

    // Construtor que recebe a lista completa de mensagens de validação
    public ErrorOnValidationException(List<string> errorMessages) : base(string.Empty)
    {
        _errors = errorMessages;
    }

    // Retorna a lista completa de erros na resposta da API
    public override List<string> GetErrors()
    {
        return _errors;
    }
}