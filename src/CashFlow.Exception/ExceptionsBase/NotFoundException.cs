using System.Net;

namespace CashFlow.Exception.ExceptionsBase;

// Exceção lançada quando um recurso não é encontrado na aplicação
public class NotFoundException : CashFlowException
{
    // Construtor que recebe uma mensagem personalizada sobre o recurso não encontrado
    public NotFoundException(string message) : base(message) { }

    public override int StatusCode => (int)HttpStatusCode.NotFound; // Define o código HTTP de resposta como 404 (Not Found) para esse tipo de exceção

    public override List<string> GetErrors() // Retorna a mensagem de erro formatada em uma lista
    {
        return [Message];
    }
}