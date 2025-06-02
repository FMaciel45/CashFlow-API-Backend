using CashFlow.Domain.Security.Cryptography;
using BC = BCrypt.Net.BCrypt; // pra simplificar o código

namespace CashFlow.Infrastructure.Security.Cryptography;

// Implementação concreta de criptografia de senhas usando BCrypt
internal class BCrypt : IPasswordEncripter
{
    // Gera um hash seguro para encriptar a senha fornecida
    public string Encrypt(string password)
    {
        string passwordHash = BC.HashPassword(password);

        return passwordHash;
    }

    // Verifica se uma senha corresponde a um hash armazenado no BD
    public bool Verify(string password, string passwordHash) => BC.Verify(password, passwordHash);
}