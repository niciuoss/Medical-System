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
  public class UserRepository : Repository<User>, IUserRepository
  {
    public UserRepository(MedicalDbContext context) : base(context)
    {
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
      return await _dbSet
          .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower() && u.IsActive);
    }

    public async Task<User?> GetByCrmAsync(string crm)
    {
      return await _dbSet
          .FirstOrDefaultAsync(u => u.Crm == crm && u.IsActive);
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
      return await _dbSet
          .AnyAsync(u => u.Email.ToLower() == email.ToLower());
    }

    public async Task<bool> CrmExistsAsync(string crm)
    {
      return await _dbSet
          .AnyAsync(u => u.Crm == crm);
    }
  }
}
