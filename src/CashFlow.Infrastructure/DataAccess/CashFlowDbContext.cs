using CashFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure.DataAccess;

// DbContext -> classe principal do Entity Framework que representa a sessão com o banco de dados
public class CashFlowDbContext : DbContext 
{
    // Construtor que recebe as opções de configuração do banco de dados
    public CashFlowDbContext(DbContextOptions options) : base(options) { } // O corpo vazio indica que toda configuração é herdada de DbContext

    public DbSet<Expense> Expenses { get; set; } // DbSet para operações com a tabela de Despesas
    public DbSet<User> Users { get; set; } // DbSet para operações com a tabela de Usuários
}