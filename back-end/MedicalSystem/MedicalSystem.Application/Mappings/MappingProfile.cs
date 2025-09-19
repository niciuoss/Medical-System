using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MedicalSystem.Application.DTOs.Patient;
using MedicalSystem.Application.DTOs.MedicalReport;
using MedicalSystem.Application.DTOs.User;
using MedicalSystem.Application.DTOs.Common;
using MedicalSystem.Domain.Entities;
using MedicalSystem.Domain.ValueObjects;

namespace MedicalSystem.Application.Mappings
{
  public class MappingProfile : Profile
  {
    public MappingProfile()
    {
      // Patient mappings
      CreateMap<CreatePatientDto, Patient>()
          .ForMember(dest => dest.Id, opt => opt.Ignore())
          .ForMember(dest => dest.UserId, opt => opt.Ignore())
          .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
          .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
          .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
          .ForMember(dest => dest.User, opt => opt.Ignore())
          .ForMember(dest => dest.MedicalReports, opt => opt.Ignore());

      CreateMap<UpdatePatientDto, Patient>()
          .ForMember(dest => dest.Id, opt => opt.Ignore())
          .ForMember(dest => dest.UserId, opt => opt.Ignore())
          .ForMember(dest => dest.Cpf, opt => opt.Ignore())
          .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
          .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
          .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
          .ForMember(dest => dest.User, opt => opt.Ignore())
          .ForMember(dest => dest.MedicalReports, opt => opt.Ignore());

      CreateMap<Patient, PatientResponseDto>()
          .ReverseMap();

      // Address mappings
      CreateMap<AddressDto, Address>()
          .ReverseMap();

      // MedicalReport mappings
      CreateMap<CreateMedicalReportDto, MedicalReport>()
          .ForMember(dest => dest.Id, opt => opt.Ignore())
          .ForMember(dest => dest.UserId, opt => opt.Ignore())
          .ForMember(dest => dest.Status, opt => opt.Ignore())
          .ForMember(dest => dest.PdfUrl, opt => opt.Ignore())
          .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
          .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
          .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
          .ForMember(dest => dest.User, opt => opt.Ignore())
          .ForMember(dest => dest.Patient, opt => opt.Ignore());

      CreateMap<MedicalReport, MedicalReportResponseDto>();

      // User mappings
      CreateMap<User, UserResponseDto>();
    }
  }
}
