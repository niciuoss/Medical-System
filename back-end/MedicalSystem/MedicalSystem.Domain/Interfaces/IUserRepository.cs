using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MedicalSystem.Domain.Entities;

namespace MedicalSystem.Domain.Interfaces
{
  public interface IUserRepository : IRepository<User>
  {
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByCrmAsync(string crm);
    Task<bool> EmailExistsAsync(string email);
    Task<bool> CrmExistsAsync(string crm);
  }
}
