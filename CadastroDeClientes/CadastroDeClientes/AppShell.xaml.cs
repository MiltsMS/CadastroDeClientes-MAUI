using CadastroDeClientes.Views;

namespace CadastroDeClientes
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            
            // Registrar rotas para navegação
            Routing.RegisterRoute("IncluirCliente", typeof(IncluirClientePage));
            Routing.RegisterRoute("AlterarCliente", typeof(AlterarClientePage));
        }
    }
}
