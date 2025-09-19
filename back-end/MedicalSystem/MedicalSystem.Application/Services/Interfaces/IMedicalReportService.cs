using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedicalSystem.Application.DTOs.MedicalReport;
using MedicalSystem.Domain.Enums;

namespace MedicalSystem.Application.Services.Interfaces
{
  public interface IMedicalReportService
  {
    Task<MedicalReportResponseDto> CreateReportAsync(CreateMedicalReportDto dto, Guid userId);
    Task<MedicalReportResponseDto?> GetReportByIdAsync(Guid id, Guid userId);
    Task<IEnumerable<MedicalReportResponseDto>> GetReportsByPatientIdAsync(Guid patientId, Guid userId);
    Task<IEnumerable<MedicalReportResponseDto>> GetReportsByUserIdAsync(Guid userId);
    Task<IEnumerable<MedicalReportResponseDto>> GetRecentReportsAsync(Guid userId, int count = 10);
    Task<MedicalReportResponseDto> UpdateReportAsync(Guid id, CreateMedicalReportDto dto, Guid userId);
    Task<MedicalReportResponseDto> UpdateReportStatusAsync(Guid id, ReportStatus status, Guid userId);
    Task DeleteReportAsync(Guid id, Guid userId);
    Task<byte[]> GeneratePdfAsync(Guid reportId, Guid userId);
  }
}
