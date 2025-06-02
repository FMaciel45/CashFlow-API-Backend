using Bogus;
using CashFlow.Communication.Requests;

namespace CommonTestUtilities.Requests;

// Builder para criar objetos de RequestLoginJson para testes 
public class RequestLoginJsonBuilder
{
    public static RequestLoginJson Build()
    {
        // Cria uma nova instância do Faker para gerar dados aleatórios do tipo RequestLoginJson
        return new Faker<RequestLoginJson>()
            .RuleFor(user => user.Email, faker => faker.Internet.Email())
            .RuleFor(user => user.Password, faker => faker.Internet.Password(prefix: "!Aa1"));
    }
}