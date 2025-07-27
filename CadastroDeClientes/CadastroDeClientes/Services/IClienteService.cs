using CadastroDeClientes.Models;
using System.Collections.ObjectModel;

namespace CadastroDeClientes.Services;

public interface IClienteService
{
    event EventHandler? ClientesChanged;
    
    Task<ObservableCollection<Cliente>> GetAllClientesAsync();
    Task<Cliente?> GetClienteByIdAsync(int id);
    Task<bool> AddClienteAsync(Cliente cliente);
    Task<bool> UpdateClienteAsync(Cliente cliente);
    Task<bool> DeleteClienteAsync(int id);
}
