using AutoMapper;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Services.LoggedUser;

namespace CashFlow.Application.UseCases.Expenses.GetAll;
public class GetAllExpenseUseCase : IGetAllExpenseUseCase
{
    private readonly IExpensesReadOnlyRepository _repository;
    private readonly IMapper _mapper; // Injeção de dependências para mapeamento dos objetos
    private readonly ILoggedUser _loggedUser;

    public GetAllExpenseUseCase(IExpensesReadOnlyRepository repository, IMapper mapper, ILoggedUser loggedUser)
    {
        _repository = repository;
        _mapper = mapper;
        _loggedUser = loggedUser;
    }

    public async Task<ResponseExpensesJson> Execute()
    {
        // Obtém o usuário logado
        var loggedUser = await _loggedUser.Get();

        // Obtém todas as despesas
        var result = await _repository.GetAll(loggedUser);

        return new ResponseExpensesJson
        {
            // Mapeia List<Expense> para List<ResponseShortExpenseJson>
            Expenses = _mapper.Map<List<ResponseShortExpenseJson>>(result)
        };
    }
}