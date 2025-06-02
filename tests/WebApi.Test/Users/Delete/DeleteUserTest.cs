using FluentAssertions;
using System.Net; // Contém as classes e estruturas necessárias para trabalhar com redes, como IP, DNS, HTTP, TCP e UDP

namespace WebApi.Test.Users.Delete;
public class DeleteUserTest : CashFlowClassFixture
{
    private const string METHOD = "api/User"; // Endpoint da API para exclusão de usuário

    private readonly string _token;     // Armazena o token de autenticação para os testes

    // Construtor que inicializa os dados de teste
    public DeleteUserTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        // Obtém o token do usuário de teste (membro do time)
        _token = webApplicationFactory.User_Team_Member.GetToken();
    }

    [Fact]
    public async Task Success()
    {
        // Chama o endpoint DELETE para excluir o usuário
        var result = await DoDelete(METHOD, _token);

        result.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}