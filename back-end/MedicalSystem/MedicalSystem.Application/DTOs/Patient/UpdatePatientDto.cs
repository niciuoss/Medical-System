using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedicalSystem.Application.DTOs.Common;

namespace MedicalSystem.Application.DTOs.Patient
{
  public class UpdatePatientDto
  {
    public string FullName { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public AddressDto? Address { get; set; }
    public string? HealthPlan { get; set; }
    public string? Allergies { get; set; }
  }
}