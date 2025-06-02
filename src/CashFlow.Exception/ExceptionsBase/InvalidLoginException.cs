using System.Net;

namespace CashFlow.Exception.ExceptionsBase;

// Exceção lançada quando as credenciais de login são inválidas
public class InvalidLoginException : CashFlowException
{
    // Construtor que inicializa com mensagem padrão de credenciais inválidas (mensagem genérica -> + segurança)
    public InvalidLoginException() : base(ResourceErrorMessages.EMAIL_OR_PASSWORD_INVALID) { } // Corpo vazio - toda inicialização é feita pela classe base

    public override int StatusCode => (int)HttpStatusCode.Unauthorized; // Define o código HTTP de resposta como 401 (Unauthorized) para esse tipo de exceção 

    public override List<string> GetErrors() // Retorna a mensagem de erro formatada em lista
    {
        return [Message];
    }
}