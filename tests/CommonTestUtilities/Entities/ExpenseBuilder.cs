using Bogus; // Gerar dados falsos realistas (faker), facilitando testes e desenvolvimento
using CashFlow.Domain.Entities;
using CashFlow.Domain.Enums;

namespace CommonTestUtilities.Entities;

// Builder para criação de entidades Expense em testes
public class ExpenseBuilder
{
    // Cria uma coleção de despesas para um usuário específico

    public static List<Expense> Collection(User user, uint count = 2) // Inicializa com a quantidade de despesas a gerar = 2
    {
        var list = new List<Expense>();

        if (count == 0) 
        {
            count = 1; // Garante mínimo de 1 item
        }

        var expenseId = 1; // Contador para IDs sequenciais iniciado em 1

        for (int i = 0; i < count; i++) // Cria múltiplas instâncias de Expense
        {
            var expense = Build(user);
            expense.Id = expenseId++; // Atribui ID sequencial às despesas

            list.Add(expense);
        }

        return list; // Lista com as despesas
    }

    // Cria uma única despesa com dados aleatórios válidos 
    public static Expense Build(User user)
    {
        return new Faker<Expense>() // Configuração do Bogus
            .RuleFor(u => u.Id, _ => 1) // Valor padrão
            .RuleFor(u => u.Title, faker => faker.Commerce.ProductName())
            .RuleFor(r => r.Description, faker => faker.Commerce.ProductDescription())
            .RuleFor(r => r.Date, faker => faker.Date.Past())
            .RuleFor(r => r.Amount, faker => faker.Random.Decimal(min: 1, max: 1000))
            .RuleFor(r => r.PaymentType, faker => faker.PickRandom<PaymentType>())
            .RuleFor(r => r.UserId, _ => user.Id);
    }
}