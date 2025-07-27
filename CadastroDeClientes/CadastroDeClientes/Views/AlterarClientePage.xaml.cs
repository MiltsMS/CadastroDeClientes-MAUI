using CadastroDeClientes.ViewModels;

namespace CadastroDeClientes.Views;

public partial class AlterarClientePage : ContentPage
{
    public AlterarClientePage(AlterarClienteViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
