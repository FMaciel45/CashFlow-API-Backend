using Bogus;
using CashFlow.Communication.Requests;

namespace CommonTestUtilities.Requests;

// Builder para criar objetos de RequestUpdateUserJson para testes 
public class RequestUpdateUserJsonBuilder
{
    public static RequestUpdateUserJson Build()
    {
        // Cria uma nova instância do Faker para gerar dados aleatórios do tipo RequestUpdateUserJson
        return new Faker<RequestUpdateUserJson>()
            .RuleFor(user => user.Name, faker => faker.Person.FirstName)
            .RuleFor(user => user.Email, (faker, user) => faker.Internet.Email(user.Name));
    }
}