using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using MedicalSystem.Application.DTOs.Patient;

namespace MedicalSystem.Application.Validators
{
  public class CreatePatientValidator : AbstractValidator<CreatePatientDto>
  {
    public CreatePatientValidator()
    {
      RuleFor(x => x.FullName)
          .NotEmpty().WithMessage("Nome é obrigatório")
          .MaximumLength(255).WithMessage("Nome deve ter no máximo 255 caracteres")
          .MinimumLength(2).WithMessage("Nome deve ter pelo menos 2 caracteres");

      RuleFor(x => x.Cpf)
          .NotEmpty().WithMessage("CPF é obrigatório")
          .Must(BeValidCpf).WithMessage("CPF deve ter formato válido")
          .Length(11).WithMessage("CPF deve ter 11 dígitos");

      RuleFor(x => x.Gender)
          .NotEmpty().WithMessage("Gênero é obrigatório")
          .Must(x => x == "masculino" || x == "feminino")
          .WithMessage("Gênero deve ser 'masculino' ou 'feminino'");

      RuleFor(x => x.Email)
          .EmailAddress()
          .When(x => !string.IsNullOrEmpty(x.Email))
          .WithMessage("Email deve ter formato válido");

      RuleFor(x => x.Phone)
          .Matches(@"^\(\d{2}\)\s\d{4,5}-\d{4}$")
          .When(x => !string.IsNullOrEmpty(x.Phone))
          .WithMessage("Telefone deve ter formato válido: (11) 99999-9999");

      RuleFor(x => x.HealthPlan)
          .MaximumLength(255)
          .When(x => !string.IsNullOrEmpty(x.HealthPlan))
          .WithMessage("Nome do convênio deve ter no máximo 255 caracteres");

      When(x => x.Address != null, () => {
        RuleFor(x => x.Address!.ZipCode)
            .Matches(@"^\d{5}-?\d{3}$")
            .When(x => !string.IsNullOrEmpty(x.Address!.ZipCode))
            .WithMessage("CEP deve ter formato válido: 00000-000");
      });
    }

    private bool BeValidCpf(string cpf)
    {
      if (string.IsNullOrEmpty(cpf) || cpf.Length != 11)
        return false;

      if (!cpf.All(char.IsDigit))
        return false;

      // Verificar se todos os dígitos são iguais
      if (cpf.Distinct().Count() == 1)
        return false;

      // Validação matemática do CPF
      var sum = 0;
      for (int i = 0; i < 9; i++)
        sum += int.Parse(cpf[i].ToString()) * (10 - i);

      var remainder = sum % 11;
      var checkDigit1 = remainder < 2 ? 0 : 11 - remainder;

      if (int.Parse(cpf[9].ToString()) != checkDigit1)
        return false;

      sum = 0;
      for (int i = 0; i < 10; i++)
        sum += int.Parse(cpf[i].ToString()) * (11 - i);

      remainder = sum % 11;
      var checkDigit2 = remainder < 2 ? 0 : 11 - remainder;

      return int.Parse(cpf[10].ToString()) == checkDigit2;
    }
  }
}
