using CashFlow.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Users.Register;
public class RegisterUserTest : CashFlowClassFixture
{
    private const string METHOD = "api/User"; // Endpoint da API para registro

    public RegisterUserTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    { } // O construtor está vazio porque os testes usam dados gerados dinamicamente

    [Fact]
    public async Task Success()
    {
        var request = RequestRegisterUserJsonBuilder.Build();

        var result = await DoPost(METHOD, request);

        result.StatusCode.Should().Be(HttpStatusCode.Created);

        // Extrai e faz um parse no corpo da resposta
        var body = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(body);

        // Verifica nome e token
        response.RootElement.GetProperty("name").GetString().Should().Be(request.Name);
        response.RootElement.GetProperty("token").GetString().Should().NotBeNullOrEmpty();
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task ErrorEmptyName(string culture)
    {
        // Cria requisição com nome vazio
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Name = string.Empty; // Para forçar erro

        // Envia requisição com a cultura especificada
        var result = await DoPost(requestUri: METHOD, request: request, culture: culture);

        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        // Processa a resposta de erro
        var body = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(body);

        // Obtém a lista de erros
        var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray();

        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("NAME_EMPTY", new CultureInfo(culture));

        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }
}