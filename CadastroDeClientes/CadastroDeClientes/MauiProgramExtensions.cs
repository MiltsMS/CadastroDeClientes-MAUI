using Microsoft.Extensions.Logging;
using CadastroDeClientes.Services;
using CadastroDeClientes.ViewModels;
using CadastroDeClientes.Views;

namespace CadastroDeClientes
{
    public static class MauiProgramExtensions
    {
        public static MauiAppBuilder UseSharedMauiApp(this MauiAppBuilder builder)
        {
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Registrar serviços
            builder.Services.AddSingleton<IDatabaseService, DatabaseService>();
            builder.Services.AddSingleton<IClienteService, ClienteService>();
            
            // Registrar ViewModels
            builder.Services.AddTransient<MainPageViewModel>();
            builder.Services.AddTransient<IncluirClienteViewModel>();
            builder.Services.AddTransient<AlterarClienteViewModel>();
            
            // Registrar Pages
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<IncluirClientePage>();
            builder.Services.AddTransient<AlterarClientePage>();

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder;
        }
    }
}
