using CadastroDeClientes.Models;
using SQLite;

namespace CadastroDeClientes.Services;

public class DatabaseService : IDatabaseService
{
    private SQLiteAsyncConnection? _database;
    private readonly string _databasePath;

    public DatabaseService()
    {
        // Define o caminho do banco de dados na pasta local da aplicação
        var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        _databasePath = Path.Combine(documentsPath, "CadastroClientes.db3");
    }

    public async Task InitializeAsync()
    {
        if (_database != null)
            return;

        try
        {
            _database = new SQLiteAsyncConnection(_databasePath);
            await _database.CreateTableAsync<Cliente>();

            // Verifica se há dados na tabela, se não houver, adiciona dados de exemplo
            var count = await _database.Table<Cliente>().CountAsync();
            if (count == 0)
            {
                await SeedDataAsync();
            }
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Erro ao inicializar banco de dados: {ex.Message}", ex);
        }
    }

    private async Task SeedDataAsync()
    {
        var clientesExemplo = new List<Cliente>
        {
            new Cliente { Name = "João", Lastname = "Silva", Age = 30, Address = "Rua das Flores, 123" },
            new Cliente { Name = "Maria", Lastname = "Santos", Age = 25, Address = "Av. Principal, 456" },
            new Cliente { Name = "Pedro", Lastname = "Oliveira", Age = 35, Address = "Rua do Comércio, 789" },
            new Cliente { Name = "Ana", Lastname = "Costa", Age = 28, Address = "Praça Central, 321" },
            new Cliente { Name = "Carlos", Lastname = "Ferreira", Age = 42, Address = "Rua Nova, 654" }
        };

        foreach (var cliente in clientesExemplo)
        {
            await _database.InsertAsync(cliente);
        }
    }

    public async Task<List<Cliente>> GetAllClientesAsync()
    {
        await InitializeAsync();
        try
        {
            return await _database!.Table<Cliente>().ToListAsync();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Erro ao buscar clientes: {ex.Message}", ex);
        }
    }

    public async Task<Cliente?> GetClienteByIdAsync(int id)
    {
        await InitializeAsync();
        try
        {
            return await _database!.Table<Cliente>().Where(c => c.Id == id).FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Erro ao buscar cliente por ID: {ex.Message}", ex);
        }
    }

    public async Task<int> SaveClienteAsync(Cliente cliente)
    {
        await InitializeAsync();
        try
        {
            if (cliente.Id != 0)
            {
                // Atualizar cliente existente
                return await _database!.UpdateAsync(cliente);
            }
            else
            {
                // Inserir novo cliente
                return await _database!.InsertAsync(cliente);
            }
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Erro ao salvar cliente: {ex.Message}", ex);
        }
    }

    public async Task<int> DeleteClienteAsync(Cliente cliente)
    {
        await InitializeAsync();
        try
        {
            return await _database!.DeleteAsync(cliente);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Erro ao excluir cliente: {ex.Message}", ex);
        }
    }

    public async Task<int> DeleteClienteByIdAsync(int id)
    {
        await InitializeAsync();
        try
        {
            return await _database!.DeleteAsync<Cliente>(id);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Erro ao excluir cliente por ID: {ex.Message}", ex);
        }
    }
}
