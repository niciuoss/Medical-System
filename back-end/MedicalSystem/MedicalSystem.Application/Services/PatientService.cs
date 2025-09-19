using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MedicalSystem.Application.DTOs.Patient;
using MedicalSystem.Application.Services.Interfaces;
using MedicalSystem.Domain.Entities;
using MedicalSystem.Domain.Interfaces;

namespace MedicalSystem.Application.Services
{
  public class PatientService : IPatientService
  {
    private readonly IPatientRepository _patientRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<CreatePatientDto> _createValidator;
    private readonly IValidator<UpdatePatientDto> _updateValidator;

    public PatientService(
        IPatientRepository patientRepository,
        IMapper mapper,
        IValidator<CreatePatientDto> createValidator,
        IValidator<UpdatePatientDto> updateValidator)
    {
      _patientRepository = patientRepository;
      _mapper = mapper;
      _createValidator = createValidator;
      _updateValidator = updateValidator;
    }

    public async Task<PatientResponseDto> CreatePatientAsync(CreatePatientDto dto, Guid userId)
    {
      var validationResult = await _createValidator.ValidateAsync(dto);
      if (!validationResult.IsValid)
      {
        throw new ValidationException(validationResult.Errors);
      }

      // Verificar se CPF já existe
      if (await _patientRepository.CpfExistsAsync(dto.Cpf))
      {
        throw new InvalidOperationException("CPF já cadastrado no sistema");
      }

      var patient = _mapper.Map<Patient>(dto);
      patient.UserId = userId;
      patient.CreatedAt = DateTime.UtcNow;
      patient.UpdatedAt = DateTime.UtcNow;

      var createdPatient = await _patientRepository.CreateAsync(patient);
      return _mapper.Map<PatientResponseDto>(createdPatient);
    }

    public async Task<PatientResponseDto?> GetPatientByIdAsync(Guid id, Guid userId)
    {
      var patient = await _patientRepository.GetByIdAsync(id);

      if (patient == null || patient.UserId != userId || patient.IsDeleted)
        return null;

      return _mapper.Map<PatientResponseDto>(patient);
    }

    public async Task<IEnumerable<PatientResponseDto>> GetPatientsByUserIdAsync(Guid userId)
    {
      var patients = await _patientRepository.GetByUserIdAsync(userId);
      return _mapper.Map<IEnumerable<PatientResponseDto>>(patients.Where(p => !p.IsDeleted));
    }

    public async Task<IEnumerable<PatientResponseDto>> SearchPatientsAsync(string searchTerm, Guid userId)
    {
      if (string.IsNullOrWhiteSpace(searchTerm))
      {
        return await GetPatientsByUserIdAsync(userId);
      }

      var patients = await _patientRepository.SearchAsync(searchTerm);
      var userPatients = patients.Where(p => p.UserId == userId && !p.IsDeleted);

      return _mapper.Map<IEnumerable<PatientResponseDto>>(userPatients);
    }

    public async Task<PatientResponseDto> UpdatePatientAsync(Guid id, UpdatePatientDto dto, Guid userId)
    {
      var validationResult = await _updateValidator.ValidateAsync(dto);
      if (!validationResult.IsValid)
      {
        throw new ValidationException(validationResult.Errors);
      }

      var existingPatient = await _patientRepository.GetByIdAsync(id);
      if (existingPatient == null || existingPatient.UserId != userId || existingPatient.IsDeleted)
      {
        throw new InvalidOperationException("Paciente não encontrado");
      }

      _mapper.Map(dto, existingPatient);
      existingPatient.UpdatedAt = DateTime.UtcNow;

      var updatedPatient = await _patientRepository.UpdateAsync(existingPatient);
      return _mapper.Map<PatientResponseDto>(updatedPatient);
    }

    public async Task DeletePatientAsync(Guid id, Guid userId)
    {
      var patient = await _patientRepository.GetByIdAsync(id);
      if (patient == null || patient.UserId != userId)
      {
        throw new InvalidOperationException("Paciente não encontrado");
      }

      patient.IsDeleted = true;
      patient.UpdatedAt = DateTime.UtcNow;
      await _patientRepository.UpdateAsync(patient);
    }

    public async Task<bool> CpfExistsAsync(string cpf)
    {
      return await _patientRepository.CpfExistsAsync(cpf);
    }
  }
}
