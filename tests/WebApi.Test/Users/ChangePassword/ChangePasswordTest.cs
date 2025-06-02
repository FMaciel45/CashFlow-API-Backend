using CashFlow.Communication.Requests;
using CashFlow.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Users.ChangePassword;

// Herda de CashFlowClassFixture para compartilhar contexto entre testes
public class ChangePasswordTest : CashFlowClassFixture
{
    private const string METHOD = "api/User/change-password"; // Endpoint da API para alteração de senha

    // Dados do usuário para teste (injetados via construtor)
    private readonly string _token;
    private readonly string _password;
    private readonly string _email;

    public ChangePasswordTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.User_Team_Member.GetToken();
        _password = webApplicationFactory.User_Team_Member.GetPassword();
        _email = webApplicationFactory.User_Team_Member.GetEmail();
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestChangePasswordJsonBuilder.Build();
        request.Password = _password; // Define a senha atual correta

        // Chama o endpoint para alterar a senha
        var response = await DoPut(METHOD, request, _token);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Prepara requisição de login com a senha antiga
        var loginRequest = new RequestLoginJson
        {
            Email = _email,
            Password = _password,
        };

        // Tenta fazer login com a senha antiga (401 -> Unauthorized)
        response = await DoPost("api/Login", loginRequest);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        // Tenta fazer login com a nova senha
        loginRequest.Password = request.NewPassword;
        response = await DoPost("api/Login", loginRequest);
        response.StatusCode.Should().Be(HttpStatusCode.OK); // 200 (Ok)
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task ErrorPasswordDifferentCurrentPassword(string culture)
    {
        // Cria requisição com senha atual incorreta
        var request = RequestChangePasswordJsonBuilder.Build();

        // Chama o endpoint para alterar senha
        var response = await DoPut(METHOD, request, token: _token, culture: culture);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        // Extrai o corpo da resposta
        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        // Obtém a lista de mensagens de erro
        var errors = responseData.RootElement.GetProperty("errorMessages").EnumerateArray();

        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("PASSWORD_DIFFERENT_CURRENT_PASSWORD", new CultureInfo(culture));

        errors.Should().HaveCount(1).And.Contain(c => c.GetString()!.Equals(expectedMessage));
    }
}