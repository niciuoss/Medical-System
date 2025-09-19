using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using MedicalSystem.Application.DTOs.Patient;

namespace MedicalSystem.Application.Validators
{
  public class UpdatePatientValidator : AbstractValidator<UpdatePatientDto>
  {
    public UpdatePatientValidator()
    {
      RuleFor(x => x.FullName)
          .NotEmpty().WithMessage("Nome é obrigatório")
          .MaximumLength(255).WithMessage("Nome deve ter no máximo 255 caracteres")
          .MinimumLength(2).WithMessage("Nome deve ter pelo menos 2 caracteres");

      RuleFor(x => x.Gender)
          .NotEmpty().WithMessage("Gênero é obrigatório")
          .Must(x => x == "masculino" || x == "feminino")
          .WithMessage("Gênero deve ser 'masculino' ou 'feminino'");

      RuleFor(x => x.Email)
          .EmailAddress()
          .When(x => !string.IsNullOrEmpty(x.Email))
          .WithMessage("Email deve ter formato válido");

      RuleFor(x => x.Phone)
          .Matches(@"^\(\d{2}\)\s?\d{4,5}-?\d{4}$")
          .When(x => !string.IsNullOrEmpty(x.Phone))
          .WithMessage("Telefone deve ter formato válido");

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
  }
}