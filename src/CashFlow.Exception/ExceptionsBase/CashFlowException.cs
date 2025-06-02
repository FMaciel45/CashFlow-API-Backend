namespace CashFlow.Exception.ExceptionsBase;

// Classe base para todas as exceções customizadas da aplicação
public abstract class CashFlowException : SystemException
{
    // A função abaixo não implementa nada no corpo porque sua única responsabilidade é repassar a mensagem para o construtor da classe base (SystemException)
    protected CashFlowException(string message) : base(message) { } // Construtor que recebe a mensagem básica do erro 

    public abstract int StatusCode { get; } // Propriedade que define o Status Code apropriado (HTTP)

    public abstract List<string> GetErrors(); // Método que retorna os detalhes do erro (para respostas API)
}