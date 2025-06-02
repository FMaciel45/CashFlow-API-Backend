using AutoMapper;
using CashFlow.Application.AutoMapper;

namespace CommonTestUtilities.Mapper;

// Builder para criar uma instância de IMapper configurada para testes
public class MapperBuilder
{
    // Cria e configura um mapper com os perfis definidos na aplicação
    public static IMapper Build()
    {
        var mapper = new MapperConfiguration(config =>
        {
            // Adiciona o perfil de mapeamento da aplicação
            config.AddProfile(new AutoMapping());
        });

        return mapper.CreateMapper();
    }
}