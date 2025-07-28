using CadastroDeClientes.ViewModels;

namespace CadastroDeClientes.Views;

public partial class AlterarClientePage : ContentPage
{
    private AlterarClienteViewModel ViewModel => (AlterarClienteViewModel)BindingContext;
    
    public AlterarClientePage(AlterarClienteViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
    
    private void OnNameUnfocused(object sender, FocusEventArgs e)
    {
        ViewModel.OnNameUnfocused();
    }
    
    private void OnLastnameUnfocused(object sender, FocusEventArgs e)
    {
        ViewModel.OnLastnameUnfocused();
    }
    
    private void OnAgeUnfocused(object sender, FocusEventArgs e)
    {
        ViewModel.OnAgeUnfocused();
    }
    
    private void OnAddressUnfocused(object sender, FocusEventArgs e)
    {
        ViewModel.OnAddressUnfocused();
    }
}
