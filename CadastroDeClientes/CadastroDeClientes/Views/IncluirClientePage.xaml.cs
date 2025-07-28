using CadastroDeClientes.ViewModels;

namespace CadastroDeClientes.Views;

public partial class IncluirClientePage : ContentPage
{
    private IncluirClienteViewModel ViewModel => (IncluirClienteViewModel)BindingContext;
    
    public IncluirClientePage(IncluirClienteViewModel viewModel)
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
