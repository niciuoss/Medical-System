using Microsoft.AspNetCore.Mvc;
using MedicalSystem.Application.DTOs.Patient;
using MedicalSystem.Application.Services.Interfaces;
using FluentValidation;

namespace MedicalSystem.API.Controllers
{
  [ApiController]
  [Route("api/v1/[controller]")]
  public class PatientsController : ControllerBase
  {
    private readonly IPatientService _patientService;

    public PatientsController(IPatientService patientService)
    {
      _patientService = patientService;
    }

    /// <summary>
    /// Criar novo paciente
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<PatientResponseDto>> CreatePatient(CreatePatientDto dto)
    {
      try
      {
        // Buscar o usuário automaticamente
        var userService = HttpContext.RequestServices.GetRequiredService<IUserService>();
        var user = await userService.GetUserByEmailAsync("doutor@medico.local");

        if (user == null)
        {
          return BadRequest(new { message = "Usuário médico não encontrado no sistema" });
        }

        var patient = await _patientService.CreatePatientAsync(dto, user.Id);
        return CreatedAtAction(nameof(GetPatient), new { id = patient.Id }, patient);
      }
      catch (ValidationException ex)
      {
        return BadRequest(new { errors = ex.Errors.Select(e => e.ErrorMessage) });
      }
      catch (InvalidOperationException ex)
      {
        return BadRequest(new { message = ex.Message });
      }
    }

    /// <summary>
    /// Buscar paciente por ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<PatientResponseDto>> GetPatient(Guid id)
    {
      try
      {
        // Buscar o usuário automaticamente
        var userService = HttpContext.RequestServices.GetRequiredService<IUserService>();
        var user = await userService.GetUserByEmailAsync("doutor@medico.local");

        if (user == null)
        {
          return BadRequest(new { message = "Usuário médico não encontrado no sistema" });
        }

        var patient = await _patientService.GetPatientByIdAsync(id, user.Id);
        return patient == null ? NotFound() : Ok(patient);
      }
      catch (Exception ex)
      {
        return StatusCode(500, new { message = "Erro interno do servidor", detail = ex.Message });
      }
    }

    /// <summary>
    /// Listar todos os pacientes do médico
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PatientResponseDto>>> GetPatients()
    {
      try
      {
        // Buscar o usuário automaticamente
        var userService = HttpContext.RequestServices.GetRequiredService<IUserService>();
        var user = await userService.GetUserByEmailAsync("doutor@medico.local");

        if (user == null)
        {
          return BadRequest(new { message = "Usuário médico não encontrado no sistema" });
        }

        var patients = await _patientService.GetPatientsByUserIdAsync(user.Id);
        return Ok(patients);
      }
      catch (Exception ex)
      {
        return StatusCode(500, new { message = "Erro interno do servidor", detail = ex.Message });
      }
    }

    /// <summary>
    /// Buscar pacientes por nome ou CPF
    /// </summary>
    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<PatientResponseDto>>> SearchPatients([FromQuery] string q)
    {
      try
      {
        // Buscar o usuário automaticamente
        var userService = HttpContext.RequestServices.GetRequiredService<IUserService>();
        var user = await userService.GetUserByEmailAsync("doutor@medico.local");

        if (user == null)
        {
          return BadRequest(new { message = "Usuário médico não encontrado no sistema" });
        }

        var patients = await _patientService.SearchPatientsAsync(q, user.Id);
        return Ok(patients);
      }
      catch (Exception ex)
      {
        return StatusCode(500, new { message = "Erro interno do servidor", detail = ex.Message });
      }
    }

    /// <summary>
    /// Atualizar paciente
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<PatientResponseDto>> UpdatePatient(Guid id, UpdatePatientDto dto)
    {
      try
      {
        // Buscar o usuário automaticamente
        var userService = HttpContext.RequestServices.GetRequiredService<IUserService>();
        var user = await userService.GetUserByEmailAsync("doutor@medico.local");

        if (user == null)
        {
          return BadRequest(new { message = "Usuário médico não encontrado no sistema" });
        }

        var patient = await _patientService.UpdatePatientAsync(id, dto, user.Id);
        return Ok(patient);
      }
      catch (ValidationException ex)
      {
        return BadRequest(new { errors = ex.Errors.Select(e => e.ErrorMessage) });
      }
      catch (InvalidOperationException ex)
      {
        return BadRequest(new { message = ex.Message });
      }
    }

    /// <summary>
    /// Excluir paciente (soft delete)
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeletePatient(Guid id)
    {
      try
      {
        // Buscar o usuário automaticamente
        var userService = HttpContext.RequestServices.GetRequiredService<IUserService>();
        var user = await userService.GetUserByEmailAsync("doutor@medico.local");

        if (user == null)
        {
          return BadRequest(new { message = "Usuário médico não encontrado no sistema" });
        }

        await _patientService.DeletePatientAsync(id, user.Id);
        return NoContent();
      }
      catch (InvalidOperationException ex)
      {
        return BadRequest(new { message = ex.Message });
      }
    }

    /// <summary>
    /// Verificar se CPF já existe
    /// </summary>
    [HttpGet("cpf-exists/{cpf}")]
    public async Task<ActionResult<bool>> CheckCpfExists(string cpf)
    {
      try
      {
        var exists = await _patientService.CpfExistsAsync(cpf);
        return Ok(new { exists });
      }
      catch (Exception ex)
      {
        return StatusCode(500, new { message = "Erro interno do servidor", detail = ex.Message });
      }
    }
  }
}