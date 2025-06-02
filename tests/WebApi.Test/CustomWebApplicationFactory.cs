using CashFlow.Domain.Entities;
using CashFlow.Domain.Enums;
using CashFlow.Domain.Security.Cryptography;
using CashFlow.Domain.Security.Tokens;
using CashFlow.Infrastructure.DataAccess;
using CommonTestUtilities.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Test.Resources;

namespace WebApi.Test;

// Classe que herda WebApplicationFactory para configurar o servidor de testes
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    // Gerenciadores de acesso
    public ExpenseIdentityManager Expense_Admin { get; private set; } = default!;
    public ExpenseIdentityManager Expense_MemberTeam { get; private set; } = default!;
    public UserIdentityManager User_Team_Member { get; private set; } = default!;
    public UserIdentityManager User_Admin { get; private set; } = default!;

    // Configura o host da aplicação para o ambiente de teste
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test")
            .ConfigureServices(services =>
            {
                // Configura o provedor de banco de dados em memória
                var provider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

                // Configura o DbContext para usar banco em memória
                services.AddDbContext<CashFlowDbContext>(config =>
                {
                    config.UseInMemoryDatabase("InMemoryDbForTesting");
                    config.UseInternalServiceProvider(provider);
                });

                // Cria um escopo para obter serviços configurados
                var scope = services.BuildServiceProvider().CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<CashFlowDbContext>();
                var passwordEncripter = scope.ServiceProvider.GetRequiredService<IPasswordEncripter>();
                var accessTokenGenerator = scope.ServiceProvider.GetRequiredService<IAccessTokenGenerator>();

                StartDatabase(dbContext, passwordEncripter, accessTokenGenerator);
            });
    }

    // Inicializa o banco de dados com dados de teste
    private void StartDatabase(
        CashFlowDbContext dbContext,
        IPasswordEncripter passwordEncripter,
        IAccessTokenGenerator accessTokenGenerator)
    {
        // Cria usuário Team Member e sua despesa
        var userTeamMember = AddUserTeamMember(dbContext, passwordEncripter, accessTokenGenerator);
        var expenseTeamMember = AddExpenses(dbContext, userTeamMember, expenseId: 1);
        Expense_MemberTeam = new ExpenseIdentityManager(expenseTeamMember);

        // Cria usuário Admin e sua despesa
        var userAdmin = AddUserAdmin(dbContext, passwordEncripter, accessTokenGenerator);
        var expenseAdmin = AddExpenses(dbContext, userAdmin, expenseId: 2);
        Expense_Admin = new ExpenseIdentityManager(expenseAdmin);

        // Persiste todas as alterações no banco
        dbContext.SaveChanges();
    }

    // Adiciona um usuário membro de equipe ao banco de dados
    private User AddUserTeamMember(
        CashFlowDbContext dbContext,
        IPasswordEncripter passwordEncripter,
        IAccessTokenGenerator accessTokenGenerator)
    {
        // Constrói um usuário padrão usando o builder
        var user = UserBuilder.Build();
        user.Id = 1; // Define ID fixo para testes

        // Preserva a senha original antes de criptografar
        var password = user.Password;
        user.Password = passwordEncripter.Encrypt(user.Password);

        dbContext.Users.Add(user);

        var token = accessTokenGenerator.Generate(user);

        // Cria um gerenciador de identidade para uso nos testes
        User_Team_Member = new UserIdentityManager(user, password, token);

        return user;
    }

    private User AddUserAdmin(
        CashFlowDbContext dbContext,
        IPasswordEncripter passwordEncripter,
        IAccessTokenGenerator accessTokenGenerator)
    {
        // Constrói um usuário admin usando o builder
        var user = UserBuilder.Build(Roles.ADMIN);
        user.Id = 2; // Define ID fixo para testes

        var password = user.Password;
        user.Password = passwordEncripter.Encrypt(user.Password);

        dbContext.Users.Add(user);

        var token = accessTokenGenerator.Generate(user);

        User_Admin = new UserIdentityManager(user, password, token);

        return user;
    }

    private Expense AddExpenses(CashFlowDbContext dbContext, User user, long expenseId)
    {
        // Constrói uma despesa padrão associada ao usuário
        var expense = ExpenseBuilder.Build(user);
        expense.Id = expenseId; // Define ID fixo para testes

        // Adiciona a despesa ao DbContext
        dbContext.Expenses.Add(expense);

        return expense;
    }
}