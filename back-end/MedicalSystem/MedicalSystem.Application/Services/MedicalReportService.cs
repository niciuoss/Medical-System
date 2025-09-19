using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MedicalSystem.Application.DTOs.MedicalReport;
using MedicalSystem.Application.Services.Interfaces;
using MedicalSystem.Domain.Entities;
using MedicalSystem.Domain.Enums;
using MedicalSystem.Domain.Interfaces;

namespace MedicalSystem.Application.Services
{
  public class MedicalReportService : IMedicalReportService
  {
    private readonly IMedicalReportRepository _reportRepository;
    private readonly IPatientRepository _patientRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateMedicalReportDto> _validator;

    public MedicalReportService(
        IMedicalReportRepository reportRepository,
        IPatientRepository patientRepository,
        IMapper mapper,
        IValidator<CreateMedicalReportDto> validator)
    {
      _reportRepository = reportRepository;
      _patientRepository = patientRepository;
      _mapper = mapper;
      _validator = validator;
    }

    public async Task<MedicalReportResponseDto> CreateReportAsync(CreateMedicalReportDto dto, Guid userId)
    {
      var validationResult = await _validator.ValidateAsync(dto);
      if (!validationResult.IsValid)
      {
        throw new ValidationException(validationResult.Errors);
      }

      // Verificar se paciente existe e pertence ao usuário
      var patient = await _patientRepository.GetByIdAsync(dto.PatientId);
      if (patient == null || patient.UserId != userId || patient.IsDeleted)
      {
        throw new InvalidOperationException("Paciente não encontrado");
      }

      var report = _mapper.Map<MedicalReport>(dto);
      report.UserId = userId;
      report.Status = ReportStatus.Draft;
      report.CreatedAt = DateTime.UtcNow;
      report.UpdatedAt = DateTime.UtcNow;

      var createdReport = await _reportRepository.CreateAsync(report);
      var result = _mapper.Map<MedicalReportResponseDto>(createdReport);

      return result;
    }

    public async Task<MedicalReportResponseDto?> GetReportByIdAsync(Guid id, Guid userId)
    {
      var report = await _reportRepository.GetByIdAsync(id);

      if (report == null || report.UserId != userId || report.IsDeleted)
        return null;

      return _mapper.Map<MedicalReportResponseDto>(report);
    }

    public async Task<IEnumerable<MedicalReportResponseDto>> GetReportsByPatientIdAsync(Guid patientId, Guid userId)
    {
      // Verificar se paciente pertence ao usuário
      var patient = await _patientRepository.GetByIdAsync(patientId);
      if (patient == null || patient.UserId != userId)
      {
        return Enumerable.Empty<MedicalReportResponseDto>();
      }

      var reports = await _reportRepository.GetByPatientIdAsync(patientId);
      var userReports = reports.Where(r => r.UserId == userId && !r.IsDeleted);

      return _mapper.Map<IEnumerable<MedicalReportResponseDto>>(userReports);
    }

    public async Task<IEnumerable<MedicalReportResponseDto>> GetReportsByUserIdAsync(Guid userId)
    {
      var reports = await _reportRepository.GetByUserIdAsync(userId);
      var activeReports = reports.Where(r => !r.IsDeleted);

      return _mapper.Map<IEnumerable<MedicalReportResponseDto>>(activeReports);
    }

    public async Task<IEnumerable<MedicalReportResponseDto>> GetRecentReportsAsync(Guid userId, int count = 10)
    {
      var reports = await _reportRepository.GetRecentReportsAsync(userId, count);
      var activeReports = reports.Where(r => !r.IsDeleted);

      return _mapper.Map<IEnumerable<MedicalReportResponseDto>>(activeReports);
    }

    public async Task<MedicalReportResponseDto> UpdateReportAsync(Guid id, CreateMedicalReportDto dto, Guid userId)
    {
      var validationResult = await _validator.ValidateAsync(dto);
      if (!validationResult.IsValid)
      {
        throw new ValidationException(validationResult.Errors);
      }

      var existingReport = await _reportRepository.GetByIdAsync(id);
      if (existingReport == null || existingReport.UserId != userId || existingReport.IsDeleted)
      {
        throw new InvalidOperationException("Laudo não encontrado");
      }

      _mapper.Map(dto, existingReport);
      existingReport.UpdatedAt = DateTime.UtcNow;

      var updatedReport = await _reportRepository.UpdateAsync(existingReport);
      return _mapper.Map<MedicalReportResponseDto>(updatedReport);
    }

    public async Task<MedicalReportResponseDto> UpdateReportStatusAsync(Guid id, ReportStatus status, Guid userId)
    {
      var report = await _reportRepository.GetByIdAsync(id);
      if (report == null || report.UserId != userId || report.IsDeleted)
      {
        throw new InvalidOperationException("Laudo não encontrado");
      }

      report.Status = status;
      report.UpdatedAt = DateTime.UtcNow;

      var updatedReport = await _reportRepository.UpdateAsync(report);
      return _mapper.Map<MedicalReportResponseDto>(updatedReport);
    }

    public async Task DeleteReportAsync(Guid id, Guid userId)
    {
      var report = await _reportRepository.GetByIdAsync(id);
      if (report == null || report.UserId != userId)
      {
        throw new InvalidOperationException("Laudo não encontrado");
      }

      report.IsDeleted = true;
      report.UpdatedAt = DateTime.UtcNow;
      await _reportRepository.UpdateAsync(report);
    }

    public async Task<byte[]> GeneratePdfAsync(Guid reportId, Guid userId)
    {
      //await Task.CompletedTask;
      throw new NotImplementedException("Lembrar de colocar aqui posteriormente a geração de PDF");
    }
  }
}