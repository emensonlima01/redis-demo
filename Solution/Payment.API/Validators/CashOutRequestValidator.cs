using FluentValidation;

namespace Payment.API.Validators;

public class CashOutRequestValidator : AbstractValidator<CashOutRequest>
{
    public CashOutRequestValidator()
    {
        RuleFor(x => x.TransactionId)
            .NotEmpty()
            .WithMessage("TransactionId é obrigatório");

        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage("Valor deve ser maior que zero");

        RuleFor(x => x.PaymentDate)
            .GreaterThanOrEqualTo(DateTime.Today)
            .WithMessage("Data de pagamento não pode ser no passado");

        RuleFor(x => x.SourceAccount)
            .SetValidator(new BankAccountValidator())
            .WithMessage("Conta origem inválida");

        RuleFor(x => x.DestinationAccount)
            .SetValidator(new BankAccountValidator())
            .WithMessage("Conta destino inválida");
    }
}

public class BankAccountValidator : AbstractValidator<BankAccount>
{
    public BankAccountValidator()
    {
        RuleFor(x => x.BankCode)
            .NotEmpty()
            .Length(3)
            .WithMessage("Código do banco deve ter 3 dígitos");

        RuleFor(x => x.Agency)
            .NotEmpty()
            .Length(4)
            .WithMessage("Agência deve ter 4 dígitos");

        RuleFor(x => x.AgencyDigit)
            .Length(0, 1)
            .WithMessage("Dígito da agência deve ter 0 ou 1 caractere");

        RuleFor(x => x.AccountNumber)
            .NotEmpty()
            .WithMessage("Número da conta é obrigatório");

        RuleFor(x => x.AccountDigit)
            .NotEmpty()
            .Length(1)
            .WithMessage("Dígito da conta deve ter 1 caractere");

        RuleFor(x => x.AccountType)
            .NotEmpty()
            .Must(x => x == "CC" || x == "CP")
            .WithMessage("Tipo de conta deve ser CC ou CP");

        RuleFor(x => x.DocumentNumber)
            .NotEmpty()
            .Must(BeValidCpfOrCnpj)
            .WithMessage("CPF/CNPJ inválido");

        RuleFor(x => x.AccountHolderName)
            .NotEmpty()
            .MinimumLength(2)
            .WithMessage("Nome do titular é obrigatório");
    }

    private bool BeValidCpfOrCnpj(string document)
    {
        return document.Length == 11 || document.Length == 14;
    }
}