using Bogus; // faker
using CashFlow.Communication.Enums;
using CashFlow.Communication.Requests;

namespace CommonTestUtilities.Requests;

// Builder para criar objetos de RequestExpenseJson para testes 
public class RequestExpenseJsonBuilder
{
    public static RequestExpenseJson Build()
    {
        // Cria uma solicitação de despesa com dados aleatórios válidos
        return new Faker<RequestExpenseJson>()
            .RuleFor(r => r.Title, faker => faker.Commerce.ProductName())
            .RuleFor(r => r.Description, faker => faker.Commerce.ProductDescription())
            .RuleFor(r => r.Date, faker => faker.Date.Past())
            .RuleFor(r => r.PaymentType, faker => faker.PickRandom<PaymentType>())
            .RuleFor(r => r.Amount, faker => faker.Random.Decimal(min: 1, max: 1000)); // Define um intervalo para o valor da despesa
    }
}