using CadastroDeClientes.Models;

namespace CadastroDeClientes.Services;

public interface IDatabaseService
{
    Task InitializeAsync();
    Task<List<Cliente>> GetAllClientesAsync();
    Task<Cliente?> GetClienteByIdAsync(int id);
    Task<int> SaveClienteAsync(Cliente cliente);
    Task<int> DeleteClienteAsync(Cliente cliente);
    Task<int> DeleteClienteByIdAsync(int id);
}
