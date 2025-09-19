using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedicalSystem.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MedicalSystem.Infrastructure.Data
{
  public static class DbSeeder
  {
    public static async Task SeedAsync(MedicalDbContext context, IPasswordHasher<User> passwordHasher)
    {
      if (await context.Users.AnyAsync())
        return; // Já existe dados

      var defaultUser = new User
      {
        Email = "doutor@medico.local",
        FullName = "Dr. Sistema Médico",
        Crm = "123456-SP",
        IsActive = true,
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow
      };

      defaultUser.PasswordHash = passwordHasher.HashPassword(defaultUser, "MedicoSeguro2024!");

      context.Users.Add(defaultUser);
      await context.SaveChangesAsync();
    }
  }
}