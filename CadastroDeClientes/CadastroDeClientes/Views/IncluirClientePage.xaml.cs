using CadastroDeClientes.ViewModels;

namespace CadastroDeClientes.Views;

public partial class IncluirClientePage : ContentPage
{
    public IncluirClientePage(IncluirClienteViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
