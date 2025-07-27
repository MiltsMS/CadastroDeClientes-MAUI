using CadastroDeClientes.Models;
using CadastroDeClientes.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CadastroDeClientes.ViewModels;

public class MainPageViewModel : BaseViewModel
{
    private readonly IClienteService _clienteService;
    private Cliente? _selectedCliente;

    public MainPageViewModel(IClienteService clienteService)
    {
        _clienteService = clienteService;
        Title = "Cadastro de Clientes";
        
        Clientes = new ObservableCollection<Cliente>();
        
        IncluirClienteCommand = new RelayCommand(async () => await IncluirCliente());
        AlterarClienteCommand = new RelayCommand(async () => await AlterarCliente(), () => SelectedCliente != null);
        ExcluirClienteCommand = new RelayCommand(async () => await ExcluirCliente(), () => SelectedCliente != null);
        RefreshCommand = new RelayCommand(async () => await LoadClientes());
        
        _ = LoadClientes();
    }

    public ObservableCollection<Cliente> Clientes { get; }

    public Cliente? SelectedCliente
    {
        get => _selectedCliente;
        set
        {
            SetProperty(ref _selectedCliente, value);
            ((RelayCommand)AlterarClienteCommand).RaiseCanExecuteChanged();
            ((RelayCommand)ExcluirClienteCommand).RaiseCanExecuteChanged();
        }
    }

    public ICommand IncluirClienteCommand { get; }
    public ICommand AlterarClienteCommand { get; }
    public ICommand ExcluirClienteCommand { get; }
    public ICommand RefreshCommand { get; }

    public async Task LoadClientes()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            var clientes = await _clienteService.GetAllClientesAsync();
            
            Clientes.Clear();
            foreach (var cliente in clientes)
            {
                Clientes.Add(cliente);
            }
        }
        catch (Exception ex)
        {
            await Application.Current!.MainPage!.DisplayAlert("Erro", $"Erro ao carregar clientes: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task IncluirCliente()
    {
        try
        {
            // Navegar para a página de inclusão
            await Shell.Current.GoToAsync("IncluirCliente");
        }
        catch (Exception ex)
        {
            await Application.Current!.MainPage!.DisplayAlert("Erro", $"Erro ao abrir tela de inclusão: {ex.Message}", "OK");
        }
    }

    private async Task AlterarCliente()
    {
        if (SelectedCliente == null) return;

        try
        {
            // Navegar para a página de alteração passando o ID do cliente
            await Shell.Current.GoToAsync($"AlterarCliente?clienteId={SelectedCliente.Id}");
        }
        catch (Exception ex)
        {
            await Application.Current!.MainPage!.DisplayAlert("Erro", $"Erro ao abrir tela de alteração: {ex.Message}", "OK");
        }
    }

    private async Task ExcluirCliente()
    {
        if (SelectedCliente == null) return;

        try
        {
            var confirmacao = await Application.Current!.MainPage!.DisplayAlert(
                "Confirmação",
                $"Deseja realmente excluir o cliente {SelectedCliente.NomeCompleto}?",
                "Sim",
                "Não");

            if (confirmacao)
            {
                var sucesso = await _clienteService.DeleteClienteAsync(SelectedCliente.Id);
                if (sucesso)
                {
                    await LoadClientes();
                    await Application.Current!.MainPage!.DisplayAlert("Sucesso", "Cliente excluído com sucesso!", "OK");
                }
                else
                {
                    await Application.Current!.MainPage!.DisplayAlert("Erro", "Erro ao excluir cliente.", "OK");
                }
            }
        }
        catch (Exception ex)
        {
            await Application.Current!.MainPage!.DisplayAlert("Erro", $"Erro ao excluir cliente: {ex.Message}", "OK");
        }
    }
}
