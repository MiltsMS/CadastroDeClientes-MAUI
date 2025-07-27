# 📋 Cadastro de Clientes - .NET MAUI

Sistema de cadastro de clientes desenvolvido em .NET MAUI 9 para Windows, utilizando o padrão MVVM e injeção de dependência.

## 🚀 Funcionalidades

- ✅ **Incluir Cliente**: Formulário completo com validação de campos obrigatórios
- ✅ **Alterar Cliente**: Edição de dados de clientes existentes
- ✅ **Excluir Cliente**: Exclusão com confirmação de segurança
- ✅ **Listar Clientes**: Visualização em lista moderna e responsiva
- ✅ **Navegação**: Entre telas com fechamento automático
- ✅ **Validação**: Campos obrigatórios e validação de tipos

## 🏗️ Arquitetura

O projeto segue o padrão **MVVM (Model-View-ViewModel)** com:

- **Models**: Classe `Cliente` com propriedades Name, Lastname, Age, Address
- **Views**: Telas XAML responsivas e modernas
- **ViewModels**: Lógica de negócio e binding com as views
- **Services**: Serviço de dados em memória com operações CRUD
- **Dependency Injection**: Configuração completa de DI

## 🛠️ Tecnologias Utilizadas

- **.NET 9**
- **.NET MAUI** (Multi-platform App UI)
- **WinUI 3** (Windows UI Library)
- **MVVM Pattern**
- **Dependency Injection**
- **XAML** para interfaces

## 📁 Estrutura do Projeto

```
CadastroDeClientes/
├── Models/
│   └── Cliente.cs                 # Modelo de dados do cliente
├── Views/
│   ├── MainPage.xaml             # Tela principal com lista
│   ├── IncluirClientePage.xaml   # Tela de inclusão
│   └── AlterarClientePage.xaml   # Tela de alteração
├── ViewModels/
│   ├── BaseViewModel.cs          # Classe base para ViewModels
│   ├── MainPageViewModel.cs      # ViewModel da tela principal
│   ├── IncluirClienteViewModel.cs # ViewModel de inclusão
│   └── AlterarClienteViewModel.cs # ViewModel de alteração
├── Services/
│   ├── IClienteService.cs        # Interface do serviço
│   └── ClienteService.cs         # Implementação do serviço
└── MauiProgramExtensions.cs      # Configuração de DI
```

## 🔧 Pré-requisitos

- **Visual Studio 2022** (versão 17.8 ou superior)
- **.NET 9 SDK**
- **Workload do .NET MAUI** instalado
- **Windows 10/11** (versão 1809 ou superior)

## 🚀 Como Executar

### 1. Clone o repositório
```bash
git clone https://github.com/[SEU-USUARIO]/CadastroDeClientes.git
cd CadastroDeClientes
```

### 2. Restaurar dependências
```bash
dotnet restore
```

### 3. Compilar o projeto
```bash
dotnet build CadastroDeClientes\CadastroDeClientes.WinUI\CadastroDeClientes.WinUI.csproj -p:Platform=x64
```

### 4. Executar a aplicação
```bash
dotnet run --project CadastroDeClientes\CadastroDeClientes.WinUI\CadastroDeClientes.WinUI.csproj -p:Platform=x64
```

### Alternativa - Visual Studio
1. Abra o arquivo `CadastroDeClientes.sln` no Visual Studio
2. Defina `CadastroDeClientes.WinUI` como projeto de inicialização
3. Selecione a plataforma `x64`
4. Pressione `F5` para executar

## 📱 Como Usar

### Tela Principal
- **Lista de Clientes**: Visualize todos os clientes cadastrados
- **Botão Incluir**: Adicione um novo cliente
- **Botão Alterar**: Edite um cliente selecionado
- **Botão Excluir**: Remove um cliente com confirmação

### Incluir Cliente
1. Clique em "Incluir" na tela principal
2. Preencha todos os campos obrigatórios:
   - Nome
   - Sobrenome
   - Idade (apenas números)
   - Endereço
3. Clique em "Salvar" ou "Cancelar"

### Alterar Cliente
1. Selecione um cliente na lista
2. Clique em "Alterar"
3. Modifique os dados desejados
4. Clique em "Salvar" ou "Cancelar"

### Excluir Cliente
1. Selecione um cliente na lista
2. Clique em "Excluir"
3. Confirme a exclusão no diálogo

## 🎯 Características Técnicas

- **Dados em Memória**: Os dados são mantidos apenas durante a execução
- **Validação**: Campos obrigatórios e validação de tipos
- **Navegação**: Shell navigation com rotas configuradas
- **UI Responsiva**: Interface adaptável com temas claro/escuro
- **Tratamento de Erros**: Try-catch em todas as operações
- **Confirmações**: Diálogos de confirmação para ações críticas

## 🔮 Funcionalidades Futuras (Opcionais)

- [ ] **Persistência de Dados**: SQLite ou JSON para salvar dados permanentemente
- [ ] **Testes Unitários**: Cobertura completa de testes
- [ ] **Validações Avançadas**: Regex para campos específicos
- [ ] **Configuração de Janela**: Maximizada e centralizada
- [ ] **Filtros e Busca**: Pesquisa por nome ou outros campos
- [ ] **Exportação**: PDF ou Excel dos dados

## 👨‍💻 Desenvolvedor

Desenvolvido como projeto de estudo em .NET MAUI seguindo as melhores práticas de desenvolvimento.

## 📄 Licença

Este projeto está sob a licença MIT. Veja o arquivo [LICENSE](LICENSE) para mais detalhes.

---

⭐ **Se este projeto foi útil para você, considere dar uma estrela!**
