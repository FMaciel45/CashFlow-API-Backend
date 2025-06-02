using Bogus;
using CashFlow.Communication.Requests;

namespace CommonTestUtilities.Requests;

// Builder para criar objetos de RequestRegisterUserJson para testes 
public class RequestRegisterUserJsonBuilder
{
    public static RequestRegisterUserJson Build()
    {
        // Cria uma nova instância do Faker para gerar dados aleatórios do tipo RequestRegisterUserJson
        return new Faker<RequestRegisterUserJson>()
            .RuleFor(user => user.Name, faker => faker.Person.FirstName)
            .RuleFor(user => user.Email, (faker, user) => faker.Internet.Email(user.Name))
            .RuleFor(user => user.Password, faker => faker.Internet.Password(prefix: "!Aa1"));
    }
}