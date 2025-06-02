using CashFlow.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CashFlow.Infrastructure.Migrations;

// Classe utilitária para aplicar migrations do banco de dados automaticamente (toda vez que o código roda)
public static class DataBaseMigration
{
    // Executa todas as migrations pendentes do banco de dados de forma assíncrona
    public async static Task MigrateDatabase(IServiceProvider serviceProvider)
    {
        // Obtém a instância do DbContext via injeção de dependências
        var dbContext = serviceProvider.GetRequiredService<CashFlowDbContext>();

        // Aplica todas as migrations pendentes
        await dbContext.Database.MigrateAsync();
    }
}