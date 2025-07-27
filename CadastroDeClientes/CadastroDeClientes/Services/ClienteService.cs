using CadastroDeClientes.Models;
using System.Collections.ObjectModel;

namespace CadastroDeClientes.Services;

public class ClienteService : IClienteService
{
    private readonly List<Cliente> _clientes;
    private int _nextId = 1;

    public ClienteService()
    {
        _clientes = new List<Cliente>();
        
        // Dados de exemplo para demonstração
        _clientes.AddRange(new[]
        {
            new Cliente { Id = _nextId++, Name = "João", Lastname = "Silva", Age = 30, Address = "Rua A, 123" },
            new Cliente { Id = _nextId++, Name = "Maria", Lastname = "Santos", Age = 25, Address = "Rua B, 456" },
            new Cliente { Id = _nextId++, Name = "Pedro", Lastname = "Oliveira", Age = 35, Address = "Rua C, 789" }
        });
    }

    public async Task<IEnumerable<Cliente>> GetAllClientesAsync()
    {
        await Task.Delay(10); // Simula operação assíncrona
        return _clientes.ToList();
    }

    public async Task<Cliente?> GetClienteByIdAsync(int id)
    {
        await Task.Delay(10);
        return _clientes.FirstOrDefault(c => c.Id == id);
    }

    public async Task<Cliente> AddClienteAsync(Cliente cliente)
    {
        await Task.Delay(10);
        cliente.Id = _nextId++;
        _clientes.Add(cliente);
        return cliente;
    }

    public async Task<Cliente> UpdateClienteAsync(Cliente cliente)
    {
        await Task.Delay(10);
        var existingCliente = _clientes.FirstOrDefault(c => c.Id == cliente.Id);
        if (existingCliente != null)
        {
            existingCliente.Name = cliente.Name;
            existingCliente.Lastname = cliente.Lastname;
            existingCliente.Age = cliente.Age;
            existingCliente.Address = cliente.Address;
        }
        return existingCliente ?? cliente;
    }

    public async Task<bool> DeleteClienteAsync(int id)
    {
        await Task.Delay(10);
        var cliente = _clientes.FirstOrDefault(c => c.Id == id);
        if (cliente != null)
        {
            _clientes.Remove(cliente);
            return true;
        }
        return false;
    }
}
