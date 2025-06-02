using CashFlow.Application.UseCases.Expenses.GetById;
using CashFlow.Domain.Entities;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using FluentAssertions;

namespace UseCases.Test.Expenses.GetById;
public class GetExpenseByIdUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        // Arrange
        var loggedUser = UserBuilder.Build(); // Cria usuário de teste
        var expense = ExpenseBuilder.Build(loggedUser); // Cria despesa associada ao usuário

        // Cria a instância do caso de uso com as dependências mockadas
        var useCase = CreateUseCase(loggedUser, expense);

        // Act
        var result = await useCase.Execute(expense.Id);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(expense.Id); // Verifica que o ID deve ser igual ao da despesa criada
        result.Title.Should().Be(expense.Title); // Verifica que o título deve ser igual ao da despesa criada
        result.Description.Should().Be(expense.Description); // Verifica que a descrição deve ser igual à da despesa criada
        result.Date.Should().Be(expense.Date); // Verifica que a data de criação deve ser igual à da despesa criada
        result.Amount.Should().Be(expense.Amount); // Verifica que o valor deve ser igual ao da despesa criada
        result.PaymentType.Should().Be((CashFlow.Communication.Enums.PaymentType)expense.PaymentType); // Verifica que o tipo de pagamento deve ser igual ao da despesa criada
    }

    [Fact]
    public async Task ErrorExpenseNotFound()
    {
        // Arrange
        var loggedUser = UserBuilder.Build();

        var useCase = CreateUseCase(loggedUser); // Não passa a despesa como parâmetro

        // Act
        var act = async () => await useCase.Execute(id: 1000); // ID inexistente

        // Assert
        var result = await act.Should().ThrowAsync<NotFoundException>();

        result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.EXPENSE_NOT_FOUND));
    }

    private GetExpenseByIdUseCase CreateUseCase(User user, Expense? expense = null)
    {
        // Cria repositório mockado configurado para retornar a despesa
        var repository = new ExpensesReadOnlyRepositoryBuilder().GetById(user, expense).Build();
        var mapper = MapperBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);

        return new GetExpenseByIdUseCase(repository, mapper, loggedUser);
    }
}