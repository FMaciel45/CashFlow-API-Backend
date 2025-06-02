using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace WebApi.Test;

// Classe base para testes de integração
public class CashFlowClassFixture : IClassFixture<CustomWebApplicationFactory>
{
    // Cliente HTTP configurado para fazer requisições à API
    private readonly HttpClient _httpClient;

    public CashFlowClassFixture(CustomWebApplicationFactory webApplicationFactory)
    {
        // Cria um cliente HTTP configurado para testar a aplicação
        _httpClient = webApplicationFactory.CreateClient();
    }

    // Executa uma requisição POST para a API
    protected async Task<HttpResponseMessage> DoPost(
        string requestUri,
        object request,
        string token = "",
        string culture = "en")
    {
        AuthorizeRequest(token);
        ChangeRequestCulture(culture);

        // Serializa o objeto para JSON e envia via POST
        return await _httpClient.PostAsJsonAsync(requestUri, request);
    }

    // Executa uma requisição PUT para a API
    protected async Task<HttpResponseMessage> DoPut(
        string requestUri,
        object request,
        string token,
        string culture = "en")
    {
        AuthorizeRequest(token);
        ChangeRequestCulture(culture);

        return await _httpClient.PutAsJsonAsync(requestUri, request);
    }

    // Executa uma requisição GET para a API
    protected async Task<HttpResponseMessage> DoGet(
        string requestUri,
        string token,
        string culture = "en")
    {
        AuthorizeRequest(token);
        ChangeRequestCulture(culture);

        return await _httpClient.GetAsync(requestUri);
    }

    // Executa uma requisição DELETE para a API
    protected async Task<HttpResponseMessage> DoDelete(
        string requestUri,
        string token,
        string culture = "en")
    {
        AuthorizeRequest(token);
        ChangeRequestCulture(culture);

        return await _httpClient.DeleteAsync(requestUri);
    }

    // Adiciona token de autenticação no cabeçalho da requisição
    private void AuthorizeRequest(string token)
    {
        if(string.IsNullOrWhiteSpace(token))
        {
            return; // Não adiciona autenticação se o token for vazio
        }

        // Configura o cabeçalho de autorização com o token Bearer
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    // Altera o cabeçalho Accept-Language para definir a cultura da requisição
    private void ChangeRequestCulture(string culture)
    {
        _httpClient.DefaultRequestHeaders.AcceptLanguage.Clear();
        _httpClient.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue(culture));
    }
}