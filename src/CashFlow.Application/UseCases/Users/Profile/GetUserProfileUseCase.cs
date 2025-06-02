using AutoMapper;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Services.LoggedUser;

namespace CashFlow.Application.UseCases.Users.Profile;

// Implementação do caso de uso para obtenção do perfil do usuário
public class GetUserProfileUseCase : IGetUserProfileUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IMapper _mapper;

    public GetUserProfileUseCase(ILoggedUser loggedUser, IMapper mapper)
    {
        _loggedUser = loggedUser;
        _mapper = mapper;
    }

    // Método principal que obtém e retorna o perfil
    public async Task<ResponseUserProfileJson> Execute()
    {
        var user = await _loggedUser.Get();

        // Mapeia a entidade User para o DTO de resposta
        return _mapper.Map<ResponseUserProfileJson>(user);
    }
}