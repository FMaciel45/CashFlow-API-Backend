using FluentAssertions;
using System.Net;
using System.Net.Mime;

namespace WebApi.Test.Expenses.Reports;
public class GenerateExpensesReportTest : CashFlowClassFixture
{
    private const string METHOD = "api/Report"; // Endpoint base para relatórios

    private readonly string _adminToken; // Token de usuário administrador
    private readonly string _teamMemberToken; // Token de usuário comum
    private readonly DateTime _expenseDate; // Data da despesa de teste

    // Construtor que inicializa os dados de teste
    public GenerateExpensesReportTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _adminToken = webApplicationFactory.User_Admin.GetToken(); 
        _teamMemberToken = webApplicationFactory.User_Team_Member.GetToken();
        _expenseDate = webApplicationFactory.Expense_Admin.GetDate();
    }

    [Fact]
    public async Task SuccessPdf()
    {
        // Faz uma requisição GET para gerar o relatório PDF
        var result = await DoGet(requestUri: $"{METHOD}/pdf?month={_expenseDate:Y}", token: _adminToken);

        result.StatusCode.Should().Be(HttpStatusCode.OK); // 200 -> Ok

        // Verifica o tipo de conteúdo retornado
        result.Content.Headers.ContentType.Should().NotBeNull();
        result.Content.Headers.ContentType!.MediaType.Should().Be(MediaTypeNames.Application.Pdf);
    }

    [Fact]
    public async Task SuccessExcel()
    {
        // Faz uma requisição GET para gerar o relatório Excel
        var result = await DoGet(requestUri: $"{METHOD}/excel?month={_expenseDate:Y}", token: _adminToken);

        result.StatusCode.Should().Be(HttpStatusCode.OK); 

        // Verifica o tipo de conteúdo retornado
        result.Content.Headers.ContentType.Should().NotBeNull();
        result.Content.Headers.ContentType!.MediaType.Should().Be(MediaTypeNames.Application.Octet);
    }

    [Fact]
    public async Task ErrorForbiddenUserNotAllowedExcel()
    {
        // Usuário comum tenta gerar relatório
        var result = await DoGet(requestUri: $"{METHOD}/excel?month={_expenseDate:Y}", token: _teamMemberToken);

        // Verifica status 403 (Forbidden)
        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task ErrorForbiddenUserNotAllowedPdf()
    {
        // Testa o caso de erro quando o usuário não autorizado tenta gerar PDF
        var result = await DoGet(requestUri: $"{METHOD}/pdf?month={_expenseDate:Y}", token: _teamMemberToken);

        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}