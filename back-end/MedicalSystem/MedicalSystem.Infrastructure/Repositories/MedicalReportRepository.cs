using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MedicalSystem.Domain.Entities;
using MedicalSystem.Domain.Enums;
using MedicalSystem.Domain.Interfaces;
using MedicalSystem.Infrastructure.Data;

namespace MedicalSystem.Infrastructure.Repositories
{
  public class MedicalReportRepository : Repository<MedicalReport>, IMedicalReportRepository
  {
    public MedicalReportRepository(MedicalDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<MedicalReport>> GetByPatientIdAsync(Guid patientId)
    {
      return await _dbSet
          .Include(r => r.Patient)
          .Include(r => r.User)
          .Where(r => r.PatientId == patientId && !r.IsDeleted)
          .OrderByDescending(r => r.ConsultationDate)
          .ToListAsync();
    }

    public async Task<IEnumerable<MedicalReport>> GetByUserIdAsync(Guid userId)
    {
      return await _dbSet
          .Include(r => r.Patient)
          .Include(r => r.User)
          .Where(r => r.UserId == userId && !r.IsDeleted)
          .OrderByDescending(r => r.CreatedAt)
          .ToListAsync();
    }

    public async Task<IEnumerable<MedicalReport>> GetRecentReportsAsync(Guid userId, int count = 10)
    {
      return await _dbSet
          .Include(r => r.Patient)
          .Include(r => r.User)
          .Where(r => r.UserId == userId && !r.IsDeleted)
          .OrderByDescending(r => r.CreatedAt)
          .Take(count)
          .ToListAsync();
    }

    public async Task<IEnumerable<MedicalReport>> GetByStatusAsync(ReportStatus status)
    {
      return await _dbSet
          .Include(r => r.Patient)
          .Include(r => r.User)
          .Where(r => r.Status == status && !r.IsDeleted)
          .OrderByDescending(r => r.CreatedAt)
          .ToListAsync();
    }

    public async Task<IEnumerable<MedicalReport>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
      return await _dbSet
          .Include(r => r.Patient)
          .Include(r => r.User)
          .Where(r => r.ConsultationDate >= startDate &&
                     r.ConsultationDate <= endDate &&
                     !r.IsDeleted)
          .OrderByDescending(r => r.ConsultationDate)
          .ToListAsync();
    }

    public override async Task<MedicalReport?> GetByIdAsync(Guid id)
    {
      return await _dbSet
          .Include(r => r.Patient)
          .Include(r => r.User)
          .FirstOrDefaultAsync(r => r.Id == id);
    }
  }
}
