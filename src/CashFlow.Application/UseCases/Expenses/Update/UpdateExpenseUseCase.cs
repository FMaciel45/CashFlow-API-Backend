using AutoMapper;
using CashFlow.Communication.Requests;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCases.Expenses.Update;
public class UpdateExpenseUseCase : IUpdateExpenseUseCase
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IExpensesUpdateOnlyRepository _repository;
    private readonly ILoggedUser _loggedUser;

    // Construtor que inicializa as dependências necessárias
    public UpdateExpenseUseCase(
        IMapper mapper,
        IUnitOfWork unitOfWork,
        IExpensesUpdateOnlyRepository repository,
        ILoggedUser loggedUser)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _repository = repository;
        _loggedUser = loggedUser;
    }

    public async Task Execute(long id, RequestExpenseJson request)
    {
        Validate(request); // Valida os dados recebidos (tratamento de exceções)

        var loggedUser = await _loggedUser.Get();

        // Busca a despesa no BD pelo seu ID 
        var expense = await _repository.GetById(loggedUser, id);

        // Verifica a existência da despesa no BD
        if (expense is null)
        {
            throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);
        }

        // Aplica as alterações com os dados fornecidos na entrada
        _mapper.Map(request, expense);

        // Atualiza a despesa como "modificada"
        _repository.Update(expense);

        // Confirma/persiste no BD as mudanças realizadas
        await _unitOfWork.Commit();
    }

    // Função para validar os dados de entrada com o FluentValidation
    private void Validate(RequestExpenseJson request)
    {
        // Instância da classe que realiza validação dos dados de entrada
        var validator = new ExpenseValidator();

        var result = validator.Validate(request);

        if (result.IsValid == false)
        {
            var errorMessages = result.Errors.Select(f => f.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMessages);
        }
    }
}