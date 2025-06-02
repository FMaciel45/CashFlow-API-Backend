namespace CashFlow.Domain.Security.Cryptography;

// Interface para abstração de criptografia de senhas
public interface IPasswordEncripter
{
    string Encrypt(string password); // Cria um hash seguro a partir de uma senha em texto puro
    bool Verify(string password, string passwordHash); // Verifica se uma senha em texto puro corresponde ao hash armazenado no BD
}