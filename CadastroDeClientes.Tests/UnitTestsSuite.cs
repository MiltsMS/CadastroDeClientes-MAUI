using FluentAssertions;
using Moq;
using CadastroDeClientes.Models;
using CadastroDeClientes.Services;
using CadastroDeClientes.ViewModels;
using CadastroDeClientes.Helpers;
using System.Collections.ObjectModel;

namespace CadastroDeClientes.Tests;

/// <summary>
/// Suíte completa de testes unitários para o projeto Cadastro de Clientes
/// </summary>
public class UnitTestsSuite
{
    #region Cliente Model Tests

    [Fact]
    public void Cliente_DeveTerPropriedadesCorretas()
    {
        // Arrange & Act
        var cliente = new Cliente();

        // Assert
        cliente.Should().NotBeNull();
        cliente.Id.Should().Be(0);
        cliente.Name.Should().Be(string.Empty);
        cliente.Lastname.Should().Be(string.Empty);
        cliente.Age.Should().Be(0);
        cliente.Address.Should().Be(string.Empty);
    }

    [Fact]
    public void Cliente_DevePermitirDefinirPropriedades()
    {
        // Arrange
        var cliente = new Cliente();
        const int expectedId = 1;
        const string expectedName = "João";
        const string expectedLastname = "Silva";
        const int expectedAge = 30;
        const string expectedAddress = "Rua das Flores, 123";

        // Act
        cliente.Id = expectedId;
        cliente.Name = expectedName;
        cliente.Lastname = expectedLastname;
        cliente.Age = expectedAge;
        cliente.Address = expectedAddress;

        // Assert
        cliente.Id.Should().Be(expectedId);
        cliente.Name.Should().Be(expectedName);
        cliente.Lastname.Should().Be(expectedLastname);
        cliente.Age.Should().Be(expectedAge);
        cliente.Address.Should().Be(expectedAddress);
    }

    #endregion

    #region ValidationHelper Tests

    [Theory]
    [InlineData("João", true, "")]
    [InlineData("Maria", true, "")]
    [InlineData("Ana Paula", true, "")]
    [InlineData("", false, "Nome é obrigatório.")]
    [InlineData("A", false, "Nome deve ter pelo menos 2 caracteres.")]
    [InlineData("João123", false, "Nome deve conter apenas letras e espaços.")]
    public void ValidationHelper_ValidateName_DeveValidarCorretamente(string name, bool expectedValid, string expectedMessage)
    {
        // Act
        var result = ValidationHelper.ValidateName(name);

        // Assert
        result.IsValid.Should().Be(expectedValid);
        result.ErrorMessage.Should().Be(expectedMessage);
    }

    [Theory]
    [InlineData("Silva", true, "")]
    [InlineData("Santos", true, "")]
    [InlineData("", false, "Sobrenome é obrigatório.")]
    [InlineData("S", false, "Sobrenome deve ter pelo menos 2 caracteres.")]
    [InlineData("Silva123", false, "Sobrenome deve conter apenas letras e espaços.")]
    public void ValidationHelper_ValidateLastname_DeveValidarCorretamente(string lastname, bool expectedValid, string expectedMessage)
    {
        // Act
        var result = ValidationHelper.ValidateLastname(lastname);

        // Assert
        result.IsValid.Should().Be(expectedValid);
        result.ErrorMessage.Should().Be(expectedMessage);
    }

    [Theory]
    [InlineData("25", true, "")]
    [InlineData("1", true, "")]
    [InlineData("120", true, "")]
    [InlineData("", false, "Idade é obrigatória.")]
    [InlineData("0", false, "Idade deve ser maior que 0.")]
    [InlineData("121", false, "Idade deve ser menor que 120 anos.")]
    [InlineData("abc", false, "Idade deve ser um número válido.")]
    public void ValidationHelper_ValidateAge_DeveValidarCorretamente(string age, bool expectedValid, string expectedMessage)
    {
        // Act
        var result = ValidationHelper.ValidateAge(age);

        // Assert
        result.IsValid.Should().Be(expectedValid);
        result.ErrorMessage.Should().Be(expectedMessage);
    }

    [Theory]
    [InlineData("Rua das Flores, 123", true, "")]
    [InlineData("Av. Principal, 456 - Apto 101", true, "")]
    [InlineData("", false, "Endereço é obrigatório.")]
    [InlineData("Rua", false, "Endereço deve ter pelo menos 5 caracteres.")]
    public void ValidationHelper_ValidateAddress_DeveValidarCorretamente(string address, bool expectedValid, string expectedMessage)
    {
        // Act
        var result = ValidationHelper.ValidateAddress(address);

        // Assert
        result.IsValid.Should().Be(expectedValid);
        result.ErrorMessage.Should().Be(expectedMessage);
    }

    #endregion

    #region ClienteService Tests

    [Fact]
    public async Task ClienteService_GetAllClientesAsync_DeveRetornarObservableCollection()
    {
        // Arrange
        var mockDatabaseService = new Mock<IDatabaseService>();
        var clientesEsperados = new List<Cliente>
        {
            new() { Id = 1, Name = "João", Lastname = "Silva", Age = 30, Address = "Endereço 1" },
            new() { Id = 2, Name = "Maria", Lastname = "Santos", Age = 25, Address = "Endereço 2" }
        };

        mockDatabaseService.Setup(x => x.GetAllClientesAsync())
            .ReturnsAsync(clientesEsperados);

        var clienteService = new ClienteService(mockDatabaseService.Object);

        // Act
        var result = await clienteService.GetAllClientesAsync();

        // Assert
        result.Should().BeOfType<ObservableCollection<Cliente>>();
        result.Should().HaveCount(2);
        result.Should().Contain(c => c.Name == "João" && c.Lastname == "Silva");
        result.Should().Contain(c => c.Name == "Maria" && c.Lastname == "Santos");
    }

    [Fact]
    public async Task ClienteService_AddClienteAsync_ComClienteValido_DeveRetornarTrueEDispararEvento()
    {
        // Arrange
        var mockDatabaseService = new Mock<IDatabaseService>();
        var cliente = new Cliente
        {
            Name = "João",
            Lastname = "Silva",
            Age = 30,
            Address = "Rua das Flores, 123"
        };

        mockDatabaseService.Setup(x => x.SaveClienteAsync(cliente))
            .ReturnsAsync(1);

        var clienteService = new ClienteService(mockDatabaseService.Object);
        var eventoDisparado = false;
        clienteService.ClientesChanged += (sender, args) => eventoDisparado = true;

        // Act
        var result = await clienteService.AddClienteAsync(cliente);

        // Assert
        result.Should().BeTrue();
        eventoDisparado.Should().BeTrue();
        mockDatabaseService.Verify(x => x.SaveClienteAsync(cliente), Times.Once);
    }

    [Fact]
    public async Task ClienteService_DeleteClienteAsync_ComIdExistente_DeveRetornarTrueEDispararEvento()
    {
        // Arrange
        var mockDatabaseService = new Mock<IDatabaseService>();
        const int clienteId = 1;
        mockDatabaseService.Setup(x => x.DeleteClienteByIdAsync(clienteId))
            .ReturnsAsync(1);

        var clienteService = new ClienteService(mockDatabaseService.Object);
        var eventoDisparado = false;
        clienteService.ClientesChanged += (sender, args) => eventoDisparado = true;

        // Act
        var result = await clienteService.DeleteClienteAsync(clienteId);

        // Assert
        result.Should().BeTrue();
        eventoDisparado.Should().BeTrue();
        mockDatabaseService.Verify(x => x.DeleteClienteByIdAsync(clienteId), Times.Once);
    }

    #endregion

    #region ViewModel Tests

    [Fact]
    public void MainPageViewModel_Constructor_DeveInicializarPropriedadesCorretamente()
    {
        // Arrange
        var mockClienteService = new Mock<IClienteService>();
        
        // Act
        var viewModel = new MainPageViewModel(mockClienteService.Object);

        // Assert
        viewModel.Title.Should().Be("Cadastro de Clientes");
        viewModel.Clientes.Should().NotBeNull();
        viewModel.Clientes.Should().BeEmpty();
        viewModel.SelectedCliente.Should().BeNull();
        viewModel.IncluirClienteCommand.Should().NotBeNull();
        viewModel.AlterarClienteCommand.Should().NotBeNull();
        viewModel.ExcluirClienteCommand.Should().NotBeNull();
        viewModel.RefreshCommand.Should().NotBeNull();
        
        // Cleanup
        viewModel.Dispose();
    }

    [Fact]
    public void MainPageViewModel_SelectedCliente_DeveNotificarMudanca()
    {
        // Arrange
        var mockClienteService = new Mock<IClienteService>();
        var viewModel = new MainPageViewModel(mockClienteService.Object);
        var cliente = new Cliente { Id = 1, Name = "João", Lastname = "Silva", Age = 30, Address = "Endereço" };
        var propertyChanged = false;

        viewModel.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(MainPageViewModel.SelectedCliente))
                propertyChanged = true;
        };

        // Act
        viewModel.SelectedCliente = cliente;

        // Assert
        viewModel.SelectedCliente.Should().Be(cliente);
        propertyChanged.Should().BeTrue();
        
        // Cleanup
        viewModel.Dispose();
    }

    [Fact]
    public void IncluirClienteViewModel_Constructor_DeveInicializarPropriedadesCorretamente()
    {
        // Arrange
        var mockClienteService = new Mock<IClienteService>();
        
        // Act
        var viewModel = new IncluirClienteViewModel(mockClienteService.Object);

        // Assert
        viewModel.Title.Should().Be("Incluir Cliente");
        viewModel.Name.Should().Be(string.Empty);
        viewModel.Lastname.Should().Be(string.Empty);
        viewModel.Age.Should().Be(string.Empty);
        viewModel.Address.Should().Be(string.Empty);
        viewModel.SalvarCommand.Should().NotBeNull();
        viewModel.CancelarCommand.Should().NotBeNull();
    }

    [Fact]
    public void IncluirClienteViewModel_Name_DeveNotificarMudanca()
    {
        // Arrange
        var mockClienteService = new Mock<IClienteService>();
        var viewModel = new IncluirClienteViewModel(mockClienteService.Object);
        var propertyChanged = false;

        viewModel.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(IncluirClienteViewModel.Name))
                propertyChanged = true;
        };

        // Act
        viewModel.Name = "João";

        // Assert
        viewModel.Name.Should().Be("João");
        propertyChanged.Should().BeTrue();
    }

    [Fact]
    public void AlterarClienteViewModel_Constructor_DeveInicializarPropriedadesCorretamente()
    {
        // Arrange
        var mockClienteService = new Mock<IClienteService>();
        
        // Act
        var viewModel = new AlterarClienteViewModel(mockClienteService.Object);

        // Assert
        viewModel.Title.Should().Be("Alterar Cliente");
        viewModel.Name.Should().Be(string.Empty);
        viewModel.Lastname.Should().Be(string.Empty);
        viewModel.Age.Should().Be(string.Empty);
        viewModel.Address.Should().Be(string.Empty);
        viewModel.ClienteId.Should().Be(0);
        viewModel.SalvarCommand.Should().NotBeNull();
        viewModel.CancelarCommand.Should().NotBeNull();
    }

    [Theory]
    [InlineData("João", "Silva", "30", "Rua das Flores, 123", true)]
    [InlineData("", "Silva", "30", "Rua das Flores, 123", false)]
    [InlineData("João", "", "30", "Rua das Flores, 123", false)]
    [InlineData("João", "Silva", "", "Rua das Flores, 123", false)]
    [InlineData("João", "Silva", "30", "", false)]
    public void IncluirClienteViewModel_CanSalvar_DeveValidarTodosCampos(string name, string lastname, string age, string address, bool expectedCanExecute)
    {
        // Arrange
        var mockClienteService = new Mock<IClienteService>();
        var viewModel = new IncluirClienteViewModel(mockClienteService.Object);
        
        viewModel.Name = name;
        viewModel.Lastname = lastname;
        viewModel.Age = age;
        viewModel.Address = address;

        // Act
        var canExecute = viewModel.SalvarCommand.CanExecute(null);

        // Assert
        canExecute.Should().Be(expectedCanExecute);
    }

    #endregion

    #region DatabaseService Tests

    [Fact]
    public async Task DatabaseService_InitializeAsync_DeveInicializarSemErros()
    {
        // Arrange
        var databaseService = new DatabaseService();

        // Act & Assert
        await databaseService.Invoking(ds => ds.InitializeAsync())
            .Should().NotThrowAsync();
    }

    [Fact]
    public async Task DatabaseService_SaveClienteAsync_ComNovoCliente_DeveRetornarIdValido()
    {
        // Arrange
        var databaseService = new DatabaseService();
        await databaseService.InitializeAsync();
        
        var cliente = new Cliente
        {
            Name = "João Teste",
            Lastname = "Silva Teste",
            Age = 30,
            Address = "Rua de Teste, 123"
        };

        // Act
        var result = await databaseService.SaveClienteAsync(cliente);

        // Assert
        result.Should().BeGreaterThan(0);
        // O ID do cliente deve ser atualizado com o valor retornado
        cliente.Id.Should().BeGreaterThan(0);
    }

    #endregion
}
