using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MedicalSystem.Domain.Enums;

namespace MedicalSystem.Domain.Entities
{
  public class MedicalReport
  {
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
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
    public ReportStatus Status { get; set; } = ReportStatus.Draft;
    public string? PdfUrl { get; set; }
    public bool IsDeleted { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual User User { get; set; } = null!;
    public virtual Patient Patient { get; set; } = null!;
  }
}