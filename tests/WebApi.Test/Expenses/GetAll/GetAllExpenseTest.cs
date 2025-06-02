using FluentAssertions;
using System.Net;
using System.Text.Json;

namespace WebApi.Test.Expenses.GetAll;
public class GetAllExpenseTest : CashFlowClassFixture
{
    private const string METHOD = "api/Expenses"; // Endpoint base para despesas

    private readonly string _token;

    // Construtor que inicializa os dados de teste
    public GetAllExpenseTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.User_Team_Member.GetToken();
    }

    [Fact]
    public async Task Success()
    {
        // Faz requisição GET para o endpoint de despesas
        var result = await DoGet(requestUri: METHOD, token: _token);

        // Assert -> verifica se o StatusCode é 200 (OK) 
        result.StatusCode.Should().Be(HttpStatusCode.OK);

        // Lê o corpo da resposta como stream
        var body = await result.Content.ReadAsStreamAsync();

        // Faz o parse do JSON retornado
        var response = await JsonDocument.ParseAsync(body);

        response.RootElement.GetProperty("expenses").EnumerateArray().Should().NotBeNullOrEmpty();
    }
}