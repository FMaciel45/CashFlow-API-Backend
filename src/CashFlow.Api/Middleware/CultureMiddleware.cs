using System.Globalization;

namespace CashFlow.Api.Middleware;

public class CultureMiddleware
{
    private readonly RequestDelegate _next; // Próximo middleware no pipeline

    // Construtor que armazena a referência para o próximo middleware
    public CultureMiddleware(RequestDelegate next)
    {
        _next = next; 
    }

    public async Task Invoke(HttpContext context)
    {
        // Obtém todas as culturas suportadas pelo .NET
        var supportedLanguages = CultureInfo.GetCultures(CultureTypes.AllCultures).ToList();

        // Obtém o primeiro valor do cabeçalho AcceptLanguage da requisição
        var requestedCulture = context.Request.Headers.AcceptLanguage.FirstOrDefault();

        // Define o padrão como inglês se nenhuma cultura for especificada
        var cultureInfo = new CultureInfo("en");

        // Verifica se foi solicitada uma cultura específica e se ela é suportada
        if (string.IsNullOrWhiteSpace(requestedCulture) == false
            && supportedLanguages.Exists(language => language.Name.Equals(requestedCulture)))
        {
            // Usa a cultura solicitada
            cultureInfo = new CultureInfo(requestedCulture);
        }

        // Aplica a cultura selecionada para a thread atual
        CultureInfo.CurrentCulture = cultureInfo; // Para formatação de números, datas, etc
        CultureInfo.CurrentUICulture = cultureInfo; // Para recursos/localização

        // Chamada do próximo middleware no pipeline
        await _next(context);
    }
}