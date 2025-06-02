using FluentAssertions;
using System.Net; // Para códigos de status HTTP
using System.Text.Json; // Para manipulação de JSON


namespace WebApi.Test.Users.Profile;
public class GetUserProfileTest : CashFlowClassFixture
{
    private const string METHOD = "api/User"; // Endpoint da API para obtenção do perfil


    private readonly string _token;
    private readonly string _userName;
    private readonly string _userEmail;

    // Construtor que inicializa os dados de teste
    public GetUserProfileTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.User_Team_Member.GetToken();
        _userName = webApplicationFactory.User_Team_Member.GetName();
        _userEmail = webApplicationFactory.User_Team_Member.GetEmail();
    }

    [Fact]
    public async Task Success()
    {
        var result = await DoGet(METHOD, _token);

        result.StatusCode.Should().Be(HttpStatusCode.OK);

        // Lê o corpo da resposta como stream
        var body = await result.Content.ReadAsStreamAsync();
        
        // Faz o parse do JSON retornado
        var response = await JsonDocument.ParseAsync(body);

        response.RootElement.GetProperty("name").GetString().Should().Be(_userName);
        response.RootElement.GetProperty("email").GetString().Should().Be(_userEmail);
    }
}