using CadastroDeClientes.Models;

namespace CadastroDeClientes.Services;

public interface IClienteService
{
    Task<IEnumerable<Cliente>> GetAllClientesAsync();
    Task<Cliente?> GetClienteByIdAsync(int id);
    Task<Cliente> AddClienteAsync(Cliente cliente);
    Task<Cliente> UpdateClienteAsync(Cliente cliente);
    Task<bool> DeleteClienteAsync(int id);
}
