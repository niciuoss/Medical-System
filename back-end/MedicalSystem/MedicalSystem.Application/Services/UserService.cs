using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using MedicalSystem.Application.DTOs.User;
using MedicalSystem.Application.Services.Interfaces;
using MedicalSystem.Domain.Entities;
using MedicalSystem.Domain.Interfaces;

namespace MedicalSystem.Application.Services
{
  public class UserService : IUserService
  {
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IPasswordHasher<User> _passwordHasher;

    public UserService(
        IUserRepository userRepository,
        IMapper mapper,
        IPasswordHasher<User> passwordHasher)
    {
      _userRepository = userRepository;
      _mapper = mapper;
      _passwordHasher = passwordHasher;
    }

    public async Task<UserResponseDto?> GetUserByIdAsync(Guid id)
    {
      var user = await _userRepository.GetByIdAsync(id);
      return user == null ? null : _mapper.Map<UserResponseDto>(user);
    }

    public async Task<UserResponseDto?> GetUserByEmailAsync(string email)
    {
      var user = await _userRepository.GetByEmailAsync(email);
      return user == null ? null : _mapper.Map<UserResponseDto>(user);
    }

    public async Task<bool> ValidateUserCredentialsAsync(string email, string password)
    {
      var user = await _userRepository.GetByEmailAsync(email);
      if (user == null || !user.IsActive)
        return false;

      var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
      return result == PasswordVerificationResult.Success;
    }
  }
}