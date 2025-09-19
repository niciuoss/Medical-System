using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using MedicalSystem.Domain.ValueObjects;

namespace MedicalSystem.Domain.Entities
{
  public class Patient
  {
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public Address? Address { get; set; }
    public string? HealthPlan { get; set; }
    public string? Allergies { get; set; }
    public bool IsDeleted { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual User User { get; set; } = null!;
    public virtual ICollection<MedicalReport> MedicalReports { get; set; } = new List<MedicalReport>();
  }
}