using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedicalSystem.Application.DTOs.Patient;
using MedicalSystem.Domain.Enums;

namespace MedicalSystem.Application.DTOs.MedicalReport
{
  public class MedicalReportResponseDto
  {
    public Guid Id { get; set; }
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
    public ReportStatus Status { get; set; }
    public string? PdfUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public PatientResponseDto? Patient { get; set; }
  }
}
