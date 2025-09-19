using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MedicalSystem.Domain.Entities;
using MedicalSystem.Domain.Interfaces;
using MedicalSystem.Infrastructure.Data;

namespace MedicalSystem.Infrastructure.Repositories
{
  public class PatientRepository : Repository<Patient>, IPatientRepository
  {
    public PatientRepository(MedicalDbContext context) : base(context)
    {
    }

    public async Task<Patient?> GetByCpfAsync(string cpf)
    {
      return await _dbSet
          .Include(p => p.User)
          .FirstOrDefaultAsync(p => p.Cpf == cpf && !p.IsDeleted);
    }

    public async Task<IEnumerable<Patient>> SearchAsync(string searchTerm)
    {
      var term = searchTerm.ToLower().Trim();

      return await _dbSet
          .Include(p => p.User)
          .Where(p => !p.IsDeleted && (
              p.FullName.ToLower().Contains(term) ||
              p.Cpf.Contains(term) ||
              (p.Email != null && p.Email.ToLower().Contains(term))))
          .OrderBy(p => p.FullName)
          .ToListAsync();
    }

    public async Task<IEnumerable<Patient>> GetByUserIdAsync(Guid userId)
    {
      return await _dbSet
          .Include(p => p.User)
          .Where(p => p.UserId == userId && !p.IsDeleted)
          .OrderBy(p => p.FullName)
          .ToListAsync();
    }

    public async Task<bool> CpfExistsAsync(string cpf)
    {
      return await _dbSet
          .AnyAsync(p => p.Cpf == cpf && !p.IsDeleted);
    }

    public override async Task<Patient?> GetByIdAsync(Guid id)
    {
      return await _dbSet
          .Include(p => p.User)
          .FirstOrDefaultAsync(p => p.Id == id);
    }
  }
}