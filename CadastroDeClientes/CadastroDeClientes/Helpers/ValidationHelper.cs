using System.Text.RegularExpressions;

namespace CadastroDeClientes.Helpers;

public static class ValidationHelper
{
    // Regex patterns
    private static readonly Regex NameRegex = new(@"^[a-zA-ZÀ-ÿ\s]{2,50}$", RegexOptions.Compiled);
    private static readonly Regex AddressRegex = new(@"^[a-zA-ZÀ-ÿ0-9\s,.-]{5,100}$", RegexOptions.Compiled);

    public static ValidationResult ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return new ValidationResult(false, "Nome é obrigatório.");

        if (name.Length < 2)
            return new ValidationResult(false, "Nome deve ter pelo menos 2 caracteres.");

        if (name.Length > 50)
            return new ValidationResult(false, "Nome deve ter no máximo 50 caracteres.");

        if (!NameRegex.IsMatch(name))
            return new ValidationResult(false, "Nome deve conter apenas letras e espaços.");

        return new ValidationResult(true, string.Empty);
    }

    public static ValidationResult ValidateLastname(string lastname)
    {
        if (string.IsNullOrWhiteSpace(lastname))
            return new ValidationResult(false, "Sobrenome é obrigatório.");

        if (lastname.Length < 2)
            return new ValidationResult(false, "Sobrenome deve ter pelo menos 2 caracteres.");

        if (lastname.Length > 50)
            return new ValidationResult(false, "Sobrenome deve ter no máximo 50 caracteres.");

        if (!NameRegex.IsMatch(lastname))
            return new ValidationResult(false, "Sobrenome deve conter apenas letras e espaços.");

        return new ValidationResult(true, string.Empty);
    }

    public static ValidationResult ValidateAge(string ageText)
    {
        if (string.IsNullOrWhiteSpace(ageText))
            return new ValidationResult(false, "Idade é obrigatória.");

        if (!int.TryParse(ageText, out int age))
            return new ValidationResult(false, "Idade deve ser um número válido.");

        if (age < 1)
            return new ValidationResult(false, "Idade deve ser maior que 0.");

        if (age > 120)
            return new ValidationResult(false, "Idade deve ser menor que 120 anos.");

        return new ValidationResult(true, string.Empty);
    }

    public static ValidationResult ValidateAddress(string address)
    {
        if (string.IsNullOrWhiteSpace(address))
            return new ValidationResult(false, "Endereço é obrigatório.");

        if (address.Length < 5)
            return new ValidationResult(false, "Endereço deve ter pelo menos 5 caracteres.");

        if (address.Length > 100)
            return new ValidationResult(false, "Endereço deve ter no máximo 100 caracteres.");

        if (!AddressRegex.IsMatch(address))
            return new ValidationResult(false, "Endereço contém caracteres inválidos.");

        return new ValidationResult(true, string.Empty);
    }
}

public class ValidationResult
{
    public bool IsValid { get; }
    public string ErrorMessage { get; }
    public bool ShouldShowError { get; }

    public ValidationResult(bool isValid, string errorMessage, bool shouldShowError = true)
    {
        IsValid = isValid;
        ErrorMessage = errorMessage;
        ShouldShowError = shouldShowError;
    }

    public static ValidationResult Valid() => new ValidationResult(true, string.Empty, false);
    public static ValidationResult Invalid(string message, bool shouldShow = true) => new ValidationResult(false, message, shouldShow);
}
