using CashFlow.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebApi.Test.Expenses.Update;
public class UpdateExpenseTest : CashFlowClassFixture
{
    private const string METHOD = "api/Expenses"; // Endpoint base para despesas

    private readonly string _token;
    private readonly long _expenseId;

    // Construtor que inicializa os dados de teste
    public UpdateExpenseTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.User_Team_Member.GetToken();
        _expenseId = webApplicationFactory.Expense_MemberTeam.GetId();
    }

    [Fact]
    public async Task Success()
    {
        // Cria requisição válida usando o builder
        var request = RequestExpenseJsonBuilder.Build();

        // Faz requisição (PUT) para atualizar a despesa
        var result = await DoPut(requestUri: $"{METHOD}/{_expenseId}", request: request, token: _token);

        // Verifica status 204 (No Content -> atualização bem sucedida)
        result.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task ErrorTitleEmpty(string culture)
    {
        var request = RequestExpenseJsonBuilder.Build();
        request.Title = string.Empty; // Define título vazio para forçar erro

        // Faz requisição com cultura específica
        var result = await DoPut(requestUri: $"{METHOD}/{_expenseId}", request: request, token: _token, culture: culture);

        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        // Processa a resposta de erro
        var body = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(body);

        // Obtém as mensagens de erro
        var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray();

        // Obtém a mensagem esperada para a cultura atual
        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("TITLE_REQUIRED", new CultureInfo(culture));

        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task ErrorExpenseNotFound(string culture)
    {
        var request = RequestExpenseJsonBuilder.Build();

        // Tenta atualizar uma despesa inexistente (ID 1000)
        var result = await DoPut(requestUri: $"{METHOD}/1000", request: request, token: _token, culture: culture);

        result.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray();

        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("EXPENSE_NOT_FOUND", new CultureInfo(culture));

        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }
}