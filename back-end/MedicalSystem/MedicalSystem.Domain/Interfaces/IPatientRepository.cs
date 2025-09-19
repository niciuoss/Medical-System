using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MedicalSystem.Domain.Entities;

namespace MedicalSystem.Domain.Interfaces
{
  public interface IPatientRepository : IRepository<Patient>
  {
    Task<Patient?> GetByCpfAsync(string cpf);
    Task<IEnumerable<Patient>> SearchAsync(string searchTerm);
    Task<IEnumerable<Patient>> GetByUserIdAsync(Guid userId);
    Task<bool> CpfExistsAsync(string cpf);
  }
}
