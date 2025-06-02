using CashFlow.Exception;
using FluentAssertions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Expenses.Delete;
public class DeleteExpenseTest : CashFlowClassFixture
{
    private const string METHOD = "api/Expenses"; // Endpoint base

    private readonly string _token;
    private readonly long _expenseId;

    // Construtor que inicializa os dados de teste
    public DeleteExpenseTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.User_Team_Member.GetToken();
        _expenseId = webApplicationFactory.Expense_MemberTeam.GetId();
    }

    [Fact]
    public async Task Success()
    {
        // Primeira chamada -> exclui a despesa
        var result = await DoDelete(requestUri: $"{METHOD}/{_expenseId}", token: _token);

        // Verifica status 204 (No Content)
        result.StatusCode.Should().Be(HttpStatusCode.NoContent); 

        // Verifica se a despesa foi realmente removida
        result = await DoGet(requestUri: $"{METHOD}/{_expenseId}", token: _token);

        // Deve retornar 404
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task ErrorExpenseNotFound(string culture)
    {
        // Tenta excluir despesa inexistente (ID 1000)
        var result = await DoDelete(requestUri: $"{METHOD}/1000", token: _token, culture: culture);

        // Verifica status 404 (Not Found)
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);

        // Verifica o corpo da resposta
        var body = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(body);

        // Obtém array de mensagens de erro
        var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray();

        // Obtém mensagem esperada baseada na cultura
        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("EXPENSE_NOT_FOUND", new CultureInfo(culture));

        // Verifica mensagem de erro
        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }
}