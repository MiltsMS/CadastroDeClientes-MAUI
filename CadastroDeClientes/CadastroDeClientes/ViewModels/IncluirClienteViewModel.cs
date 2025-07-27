using CadastroDeClientes.Models;
using CadastroDeClientes.Services;
using System.Windows.Input;

namespace CadastroDeClientes.ViewModels;

public class IncluirClienteViewModel : BaseViewModel
{
    private readonly IClienteService _clienteService;
    private string _name = string.Empty;
    private string _lastname = string.Empty;
    private string _age = string.Empty;
    private string _address = string.Empty;

    public IncluirClienteViewModel(IClienteService clienteService)
    {
        _clienteService = clienteService;
        Title = "Incluir Cliente";
        
        SalvarCommand = new RelayCommand(async () => await SalvarCliente(), CanSalvar);
        CancelarCommand = new RelayCommand(async () => await CancelarOperacao());
    }

    public string Name
    {
        get => _name;
        set
        {
            SetProperty(ref _name, value);
            ((RelayCommand)SalvarCommand).RaiseCanExecuteChanged();
        }
    }

    public string Lastname
    {
        get => _lastname;
        set
        {
            SetProperty(ref _lastname, value);
            ((RelayCommand)SalvarCommand).RaiseCanExecuteChanged();
        }
    }

    public string Age
    {
        get => _age;
        set
        {
            SetProperty(ref _age, value);
            ((RelayCommand)SalvarCommand).RaiseCanExecuteChanged();
        }
    }

    public string Address
    {
        get => _address;
        set
        {
            SetProperty(ref _address, value);
            ((RelayCommand)SalvarCommand).RaiseCanExecuteChanged();
        }
    }

    public ICommand SalvarCommand { get; }
    public ICommand CancelarCommand { get; }

    private bool CanSalvar()
    {
        return !string.IsNullOrWhiteSpace(Name) &&
               !string.IsNullOrWhiteSpace(Lastname) &&
               !string.IsNullOrWhiteSpace(Age) &&
               int.TryParse(Age, out var idade) && idade > 0 &&
               !string.IsNullOrWhiteSpace(Address);
    }

    private async Task SalvarCliente()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;

            if (!int.TryParse(Age, out var idade))
            {
                await Application.Current!.MainPage!.DisplayAlert("Erro", "A idade deve ser um número válido.", "OK");
                return;
            }

            var novoCliente = new Cliente
            {
                Name = Name.Trim(),
                Lastname = Lastname.Trim(),
                Age = idade,
                Address = Address.Trim()
            };

            await _clienteService.AddClienteAsync(novoCliente);
            
            await Application.Current!.MainPage!.DisplayAlert("Sucesso", "Cliente incluído com sucesso!", "OK");
            
            // Voltar para a tela principal
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            await Application.Current!.MainPage!.DisplayAlert("Erro", $"Erro ao salvar cliente: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task CancelarOperacao()
    {
        var confirmacao = await Application.Current!.MainPage!.DisplayAlert(
            "Confirmação",
            "Deseja cancelar a inclusão? Os dados não salvos serão perdidos.",
            "Sim",
            "Não");

        if (confirmacao)
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
