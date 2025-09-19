using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MedicalSystem.Domain.Entities;
using MedicalSystem.Domain.Enums;

namespace MedicalSystem.Domain.Interfaces
{
  public interface IMedicalReportRepository : IRepository<MedicalReport>
  {
    Task<IEnumerable<MedicalReport>> GetByPatientIdAsync(Guid patientId);
    Task<IEnumerable<MedicalReport>> GetByUserIdAsync(Guid userId);
    Task<IEnumerable<MedicalReport>> GetRecentReportsAsync(Guid userId, int count = 10);
    Task<IEnumerable<MedicalReport>> GetByStatusAsync(ReportStatus status);
    Task<IEnumerable<MedicalReport>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
  }
}