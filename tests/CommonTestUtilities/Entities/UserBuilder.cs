using Bogus;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Enums;
using CommonTestUtilities.Cryptography;

namespace CommonTestUtilities.Entities;

// B8ilder para a criação de entidades User em testes
public class UserBuilder
{
    // Cria um usuário com dados aleatórios válidos
    public static User Build(string role = Roles.TEAM_MEMBER)
    {
        // Cria um PasswordEncrypter com mock
        var passwordEncripter = new PasswordEncrypterBuilder().Build();

        // Configura o gerador de dados fake (Bogus)
        var user = new Faker<User>()
            .RuleFor(u => u.Id, _ => 1)
            .RuleFor(u => u.Name, faker => faker.Person.FirstName)
            .RuleFor(u => u.Email, (faker, user) => faker.Internet.Email(user.Name))
            .RuleFor(u => u.Password, (_, user) => passwordEncripter.Encrypt(user.Password))
            .RuleFor(u => u.UserIdentifier, _ => Guid.NewGuid())
            .RuleFor(u => u.Role, _ => role);

        return user;
    }
}