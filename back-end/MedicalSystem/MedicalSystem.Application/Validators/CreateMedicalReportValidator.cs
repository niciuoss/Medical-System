using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using MedicalSystem.Application.DTOs.MedicalReport;
using MedicalSystem.Domain.Enums;

namespace MedicalSystem.Application.Validators
{
  public class CreateMedicalReportValidator : AbstractValidator<CreateMedicalReportDto>
  {
    public CreateMedicalReportValidator()
    {
      RuleFor(x => x.PatientId)
          .NotEmpty().WithMessage("Paciente é obrigatório");

      RuleFor(x => x.ReportType)
          .IsInEnum().WithMessage("Tipo de laudo inválido");

      RuleFor(x => x.Diagnosis)
          .NotEmpty().WithMessage("Diagnóstico é obrigatório")
          .MaximumLength(5000).WithMessage("Diagnóstico deve ter no máximo 5000 caracteres");

      RuleFor(x => x.ConsultationDate)
          .NotEmpty().WithMessage("Data da consulta é obrigatória")
          .LessThanOrEqualTo(DateTime.Now).WithMessage("Data da consulta não pode ser futura");

      RuleFor(x => x.PathologyDuration)
          .MaximumLength(255)
          .When(x => !string.IsNullOrEmpty(x.PathologyDuration))
          .WithMessage("Tempo da patologia deve ter no máximo 255 caracteres");

      RuleFor(x => x.TreatmentPerformed)
          .MaximumLength(5000)
          .When(x => !string.IsNullOrEmpty(x.TreatmentPerformed))
          .WithMessage("Tratamento realizado deve ter no máximo 5000 caracteres");

      RuleFor(x => x.Prescription)
          .MaximumLength(5000)
          .When(x => !string.IsNullOrEmpty(x.Prescription))
          .WithMessage("Prescrição deve ter no máximo 5000 caracteres");

      RuleFor(x => x.DiseaseDisabilities)
          .MaximumLength(5000)
          .When(x => !string.IsNullOrEmpty(x.DiseaseDisabilities))
          .WithMessage("Incapacidades deve ter no máximo 5000 caracteres");

      RuleFor(x => x.DiseaseDuration)
          .MaximumLength(255)
          .When(x => !string.IsNullOrEmpty(x.DiseaseDuration))
          .WithMessage("Tempo da doença deve ter no máximo 255 caracteres");

      RuleFor(x => x.Cid10)
          .MaximumLength(10)
          .When(x => !string.IsNullOrEmpty(x.Cid10))
          .WithMessage("CID-10 deve ter no máximo 10 caracteres")
          .Matches(@"^[A-Z]\d{2}(\.\d{1,2})?$")
          .When(x => !string.IsNullOrEmpty(x.Cid10))
          .WithMessage("CID-10 deve ter formato válido (ex: A00, A00.1, A00.12)");

      RuleFor(x => x.Prognosis)
          .MaximumLength(5000)
          .When(x => !string.IsNullOrEmpty(x.Prognosis))
          .WithMessage("Prognóstico deve ter no máximo 5000 caracteres");
    }
  }
}
