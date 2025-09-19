using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedicalSystem.Application.DTOs.User;

namespace MedicalSystem.Application.Services.Interfaces
{
  public interface IUserService
  {
    Task<UserResponseDto?> GetUserByIdAsync(Guid id);
    Task<UserResponseDto?> GetUserByEmailAsync(string email);
    Task<bool> ValidateUserCredentialsAsync(string email, string password);
  }
}