using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedicalSystem.Application.DTOs.Patient;

namespace MedicalSystem.Application.Services.Interfaces
{
  public interface IPatientService
  {
    Task<PatientResponseDto> CreatePatientAsync(CreatePatientDto dto, Guid userId);
    Task<PatientResponseDto?> GetPatientByIdAsync(Guid id, Guid userId);
    Task<IEnumerable<PatientResponseDto>> GetPatientsByUserIdAsync(Guid userId);
    Task<IEnumerable<PatientResponseDto>> SearchPatientsAsync(string searchTerm, Guid userId);
    Task<PatientResponseDto> UpdatePatientAsync(Guid id, UpdatePatientDto dto, Guid userId);
    Task DeletePatientAsync(Guid id, Guid userId);
    Task<bool> CpfExistsAsync(string cpf);
  }
}
