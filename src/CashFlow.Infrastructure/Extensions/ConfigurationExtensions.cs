using Microsoft.Extensions.Configuration;

namespace CashFlow.Infrastructure.Extensions;

// Extensões para IConfiguration que adicionam funcionalidades específicas
public static class ConfigurationExtensions
{
    // Verifica se a aplicação está rodando em ambiente de teste em memória
    public static bool IsTestEnvironment(this IConfiguration configuration)
    {
        return configuration.GetValue<bool>("InMemoryTest");
        // Retorna true se estiver em ambiente de teste (propósito -> não poluir o BD com informações referentes a testes de unidade)
    }
}