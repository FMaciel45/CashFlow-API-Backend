using CashFlow.Communication.Responses;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CashFlow.Api.Filters;

public class ExceptionFilter : IExceptionFilter
{
    // Captura exceções não tratadas e padroniza respostas de erro 
    public void OnException(ExceptionContext context)
    {
        if(context.Exception is CashFlowException)
        {
            HandleProjectException(context); // Exceções conhecidas
        }
        else
        {
            ThrowUnkowError(context); // Exceções desconhecidas/inesperadas
        }
    }

    private void HandleProjectException(ExceptionContext context)
    {
        // Captura a exceção específica
        var cashFlowException = (CashFlowException)context.Exception;

        // Cria um objeto padronizado para mensagens de erro
        var errorResponse = new ResponseErrorJson(cashFlowException.GetErrors());

        // Configuração da resposta
        context.HttpContext.Response.StatusCode = cashFlowException.StatusCode;
        context.Result = new ObjectResult(errorResponse);
    }

    private void ThrowUnkowError(ExceptionContext context)
    {
        // Cria uma mensagem genérica para erros desconhecidos
        var errorResponse = new ResponseErrorJson(ResourceErrorMessages.UNKNOWN_ERROR);

        // Retorna 500 (Internal Server Error) 
        context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Result = new ObjectResult(errorResponse);
    }
}