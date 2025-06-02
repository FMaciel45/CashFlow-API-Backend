using Bogus; // faker
using CashFlow.Communication.Requests;

namespace CommonTestUtilities.Requests;

// Builder para criar objetos RequestChangePasswordJson para testagem. Objetivo: Gerar solicitações de alteração de senha com dados válidos
public class RequestChangePasswordJsonBuilder
{
    public static RequestChangePasswordJson Build()
    {
        // Cria uma solicitação de alteração de senha com dados aleatórios válidos
        return new Faker<RequestChangePasswordJson>()
            .RuleFor(user => user.Password, faker => faker.Internet.Password())
            .RuleFor(user => user.NewPassword, faker => faker.Internet.Password(prefix: "!Aa1")); // Define um prefixo padrão para todos os testes aleatórios para não possuir senha inválida
    }
}