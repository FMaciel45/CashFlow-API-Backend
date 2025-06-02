using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace CashFlow.Application.UseCases.Expenses.Delete;
public class DeleteExpenseUseCase : IDeleteExpenseUseCase
{
    // Classe que implementa a lógica do caso de uso de exclusão de despesas

    // Injeção de dependências
    private readonly IExpensesReadOnlyRepository _expensesReadOnly; // Interface para consultas
    private readonly IExpensesWriteOnlyRepository _repository; // Interface para operações de escrita
    private readonly IUnitOfWork _unitOfWork; // Interface para realizar o "Commit" das alterações realizadas
    private readonly ILoggedUser _loggedUser; // Interface para obter o usuário logado

    // Campos readonly são imutáveis após a sua construção

    // Construtor que recebe as dependências via injeção de dependências
    public DeleteExpenseUseCase(
        IExpensesWriteOnlyRepository repository,
        IExpensesReadOnlyRepository expensesReadOnly,
        IUnitOfWork unitOfWork,
        ILoggedUser loggedUser)
    {
        _repository = repository; // armazena as dependências em campos "readonly"
        _unitOfWork = unitOfWork; // armazena as dependências em campos "readonly"
        _loggedUser = loggedUser; // armazena as dependências em campos "readonly"
        _expensesReadOnly = expensesReadOnly; // armazena as dependências em campos "readonly"
    }

    // Método que executa a exclusão
    public async Task Execute(long id)
    {
        // Obtém o usuário logado
        var loggedUser = await _loggedUser.Get();

        // Busca a despesa pelo ID, verificando se pertence ao usuário
        var expense = await _expensesReadOnly.GetById(loggedUser, id);

        //Valida se a despesa existe
        if (expense is null) // Pode ser (!expense) ??? -> pesquisar
        {
            throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);
        }

        // Exclui a despesa no BD
        await _repository.Delete(id);

        // Persiste a operação realizada no BD caso não caia no "if" acima
        await _unitOfWork.Commit();
    }
}