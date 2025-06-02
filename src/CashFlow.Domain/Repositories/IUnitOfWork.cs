namespace CashFlow.Domain.Repositories;
public interface IUnitOfWork
{
    // Método para confirmar operações para um BD em uma única transação

    Task Commit();
}