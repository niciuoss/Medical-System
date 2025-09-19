using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedicalSystem.Domain.Enums;

namespace MedicalSystem.Application.DTOs.MedicalReport
{
  public class CreateMedicalReportDto
  {
    public Guid PatientId { get; set; }
    public ReportType ReportType { get; set; }
    public string? PathologyDuration { get; set; }
    public string Diagnosis { get; set; } = string.Empty;
    public string? TreatmentPerformed { get; set; }
    public string? TreatmentImageUrl { get; set; }
    public string? Prescription { get; set; }
    public string? DiseaseDisabilities { get; set; }
    public string? DiseaseDuration { get; set; }
    public string? Cid10 { get; set; }
    public string? Prognosis { get; set; }
    public string? PrognosisImageUrl { get; set; }
    public DateTime ConsultationDate { get; set; }
  }
}
