using CadastroDeClientes.Models;
using CadastroDeClientes.Services;
using CadastroDeClientes.Helpers;
using System.Windows.Input;

namespace CadastroDeClientes.ViewModels;

[QueryProperty(nameof(ClienteId), "clienteId")]
public class AlterarClienteViewModel : BaseViewModel
{
    private readonly IClienteService _clienteService;
    private int _clienteId;
    private string _name = string.Empty;
    private string _lastname = string.Empty;
    private string _age = string.Empty;
    private string _address = string.Empty;
    
    private string _nameError = string.Empty;
    private string _lastnameError = string.Empty;
    private string _ageError = string.Empty;
    private string _addressError = string.Empty;
    
    private bool _nameTouched = false;
    private bool _lastnameTouched = false;
    private bool _ageTouched = false;
    private bool _addressTouched = false;
    private bool _formSubmitted = false;

    public AlterarClienteViewModel(IClienteService clienteService)
    {
        _clienteService = clienteService;
        Title = "Alterar Cliente";
        
        SalvarCommand = new RelayCommand(async () => await SalvarCliente(), CanSalvar);
        CancelarCommand = new RelayCommand(async () => await CancelarOperacao());
    }

    public int ClienteId
    {
        get => _clienteId;
        set
        {
            SetProperty(ref _clienteId, value);
            _ = LoadCliente();
        }
    }

    public string Name
    {
        get => _name;
        set
        {
            SetProperty(ref _name, value);
            ValidateName();
            ((RelayCommand)SalvarCommand).RaiseCanExecuteChanged();
        }
    }

    public string Lastname
    {
        get => _lastname;
        set
        {
            SetProperty(ref _lastname, value);
            ValidateLastname();
            ((RelayCommand)SalvarCommand).RaiseCanExecuteChanged();
        }
    }

    public string Age
    {
        get => _age;
        set
        {
            SetProperty(ref _age, value);
            ValidateAge();
            ((RelayCommand)SalvarCommand).RaiseCanExecuteChanged();
        }
    }

    public string Address
    {
        get => _address;
        set
        {
            SetProperty(ref _address, value);
            ValidateAddress();
            ((RelayCommand)SalvarCommand).RaiseCanExecuteChanged();
        }
    }

    public string NameError
    {
        get => _nameError;
        set => SetProperty(ref _nameError, value);
    }

    public string LastnameError
    {
        get => _lastnameError;
        set => SetProperty(ref _lastnameError, value);
    }

    public string AgeError
    {
        get => _ageError;
        set => SetProperty(ref _ageError, value);
    }

    public string AddressError
    {
        get => _addressError;
        set => SetProperty(ref _addressError, value);
    }

    public ICommand SalvarCommand { get; }
    public ICommand CancelarCommand { get; }

    private async Task LoadCliente()
    {
        if (IsBusy || ClienteId <= 0) return;

        try
        {
            IsBusy = true;
            var cliente = await _clienteService.GetClienteByIdAsync(ClienteId);
            
            if (cliente != null)
            {
                Name = cliente.Name;
                Lastname = cliente.Lastname;
                Age = cliente.Age.ToString();
                Address = cliente.Address;
            }
            else
            {
                await Application.Current!.MainPage!.DisplayAlert("Erro", "Cliente não encontrado.", "OK");
                await Shell.Current.GoToAsync("..");
            }
        }
        catch (Exception ex)
        {
            await Application.Current!.MainPage!.DisplayAlert("Erro", $"Erro ao carregar cliente: {ex.Message}", "OK");
            await Shell.Current.GoToAsync("..");
        }
        finally
        {
            IsBusy = false;
        }
    }

    // Métodos para marcar campos como "tocados" quando o usuário interagir
    public void OnNameUnfocused()
    {
        _nameTouched = true;
        ValidateName();
    }
    
    public void OnLastnameUnfocused()
    {
        _lastnameTouched = true;
        ValidateLastname();
    }
    
    public void OnAgeUnfocused()
    {
        _ageTouched = true;
        ValidateAge();
    }
    
    public void OnAddressUnfocused()
    {
        _addressTouched = true;
        ValidateAddress();
    }

    private void ValidateName(bool forceShow = false)
    {
        var result = ValidationHelper.ValidateName(Name);
        var shouldShow = forceShow || _formSubmitted || _nameTouched;
        NameError = shouldShow && !result.IsValid ? result.ErrorMessage : string.Empty;
    }

    private void ValidateLastname(bool forceShow = false)
    {
        var result = ValidationHelper.ValidateLastname(Lastname);
        var shouldShow = forceShow || _formSubmitted || _lastnameTouched;
        LastnameError = shouldShow && !result.IsValid ? result.ErrorMessage : string.Empty;
    }

    private void ValidateAge(bool forceShow = false)
    {
        var result = ValidationHelper.ValidateAge(Age);
        var shouldShow = forceShow || _formSubmitted || _ageTouched;
        AgeError = shouldShow && !result.IsValid ? result.ErrorMessage : string.Empty;
    }

    private void ValidateAddress(bool forceShow = false)
    {
        var result = ValidationHelper.ValidateAddress(Address);
        var shouldShow = forceShow || _formSubmitted || _addressTouched;
        AddressError = shouldShow && !result.IsValid ? result.ErrorMessage : string.Empty;
    }

    private bool CanSalvar()
    {
        var nameValid = ValidationHelper.ValidateName(Name).IsValid;
        var lastnameValid = ValidationHelper.ValidateLastname(Lastname).IsValid;
        var ageValid = ValidationHelper.ValidateAge(Age).IsValid;
        var addressValid = ValidationHelper.ValidateAddress(Address).IsValid;

        return nameValid && lastnameValid && ageValid && addressValid;
    }

    private async Task SalvarCliente()
    {
        if (IsBusy) return;

        // Marcar formulário como submetido para mostrar todas as validações
        _formSubmitted = true;
        
        // Forçar validação de todos os campos
        ValidateName(true);
        ValidateLastname(true);
        ValidateAge(true);
        ValidateAddress(true);
        
        // Verificar se há erros de validação
        if (!CanSalvar())
        {
            await Application.Current!.MainPage!.DisplayAlert("Erro", "Por favor, corrija os erros antes de salvar.", "OK");
            return;
        }

        try
        {
            IsBusy = true;

            if (!int.TryParse(Age, out var idade))
            {
                await Application.Current!.MainPage!.DisplayAlert("Erro", "A idade deve ser um número válido.", "OK");
                return;
            }

            var clienteAtualizado = new Cliente
            {
                Id = ClienteId,
                Name = Name.Trim(),
                Lastname = Lastname.Trim(),
                Age = idade,
                Address = Address.Trim()
            };

            await _clienteService.UpdateClienteAsync(clienteAtualizado);
            
            await Application.Current!.MainPage!.DisplayAlert("Sucesso", "Cliente alterado com sucesso!", "OK");
            
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
            "Deseja cancelar a alteração? As modificações não salvas serão perdidas.",
            "Sim",
            "Não");

        if (confirmacao)
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
