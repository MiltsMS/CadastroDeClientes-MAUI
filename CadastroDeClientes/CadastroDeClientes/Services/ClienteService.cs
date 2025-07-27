using CadastroDeClientes.Models;
using System.Collections.ObjectModel;

namespace CadastroDeClientes.Services;

public class ClienteService : IClienteService
{
    private readonly IDatabaseService _databaseService;

    public event EventHandler? ClientesChanged;

    public ClienteService(IDatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    private void OnClientesChanged()
    {
        ClientesChanged?.Invoke(this, EventArgs.Empty);
    }

    public async Task<ObservableCollection<Cliente>> GetAllClientesAsync()
    {
        try
        {
            await _databaseService.InitializeAsync();
            var clientes = await _databaseService.GetAllClientesAsync();
            return new ObservableCollection<Cliente>(clientes);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Erro ao buscar clientes: {ex.Message}", ex);
        }
    }

    public async Task<Cliente?> GetClienteByIdAsync(int id)
    {
        try
        {
            var cliente = await _databaseService.GetClienteByIdAsync(id);
            return cliente;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Erro ao buscar cliente por ID: {ex.Message}", ex);
        }
    }

    public async Task<bool> AddClienteAsync(Cliente cliente)
    {
        try
        {
            if (cliente == null)
                throw new ArgumentNullException(nameof(cliente));

            var result = await _databaseService.SaveClienteAsync(cliente);
            if (result > 0)
            {
                OnClientesChanged();
            }
            return result > 0;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Erro ao adicionar cliente: {ex.Message}", ex);
        }
    }

    public async Task<bool> UpdateClienteAsync(Cliente cliente)
    {
        try
        {
            if (cliente == null)
                throw new ArgumentNullException(nameof(cliente));

            var result = await _databaseService.SaveClienteAsync(cliente);
            if (result > 0)
            {
                OnClientesChanged();
            }
            return result > 0;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Erro ao atualizar cliente: {ex.Message}", ex);
        }
    }

    public async Task<bool> DeleteClienteAsync(int id)
    {
        try
        {
            var result = await _databaseService.DeleteClienteByIdAsync(id);
            if (result > 0)
            {
                OnClientesChanged();
            }
            return result > 0;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Erro ao excluir cliente: {ex.Message}", ex);
        }
    }
}
