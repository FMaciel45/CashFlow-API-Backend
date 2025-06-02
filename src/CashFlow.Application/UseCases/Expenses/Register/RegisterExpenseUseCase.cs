using AutoMapper;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCases.Expenses.Register;
public class RegisterExpenseUseCase : IRegisterExpenseUseCase
{
    private readonly IExpensesWriteOnlyRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILoggedUser _loggedUser;

    // Construtor com as injeções de dependências
    public RegisterExpenseUseCase(
        IExpensesWriteOnlyRepository repository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILoggedUser loggedUser)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _loggedUser = loggedUser;
    }

    // Executa o fluxo completo do registro de despesa
    public async Task<ResponseRegisteredExpenseJson> Execute(RequestExpenseJson request)
    {
        // Valida os dados de entrada (tratamento de exceções)
        Validate(request);

        // Obtém o usuário logado
        var loggedUser = await _loggedUser.Get();

        // Mapeia o DTO (Data Transfer Object) de requisição para a entidade Expense
        var expense = _mapper.Map<Expense>(request);
        expense.UserId = loggedUser.Id; // problema resolvido -> atrelar o UserId à despesa 

        // Adiciona a despesa no repositório
        await _repository.Add(expense);

        // Persiste as alterações no BD
        await _unitOfWork.Commit();

        // Retorna o DTO de resposta mapeado da entidade
        return _mapper.Map<ResponseRegisteredExpenseJson>(expense);
    }

    private void Validate(RequestExpenseJson request)
    {
        // Validador que valida a entrada e associa as exceções às mensagens de erro
        var validator = new ExpenseValidator();

        // Validação dos dados de entrada
        var result = validator.Validate(request);

        // Se os dados de entrada não forem válidos 
        if (result.IsValid == false) // Pode ser (!result.IsValid) ???
        {
            // Extrai as mensagens de erro
            var errorMessages = result.Errors.Select(f => f.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMessages);
        }
    }
}