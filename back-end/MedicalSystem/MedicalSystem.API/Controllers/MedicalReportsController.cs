using Microsoft.AspNetCore.Mvc;
using MedicalSystem.Application.DTOs.MedicalReport;
using MedicalSystem.Application.Services.Interfaces;
using MedicalSystem.Domain.Enums;
using FluentValidation;

namespace MedicalSystem.API.Controllers
{
  [ApiController]
  [Route("api/v1/[controller]")]
  public class MedicalReportsController : ControllerBase
  {
    private readonly IMedicalReportService _reportService;

    public MedicalReportsController(IMedicalReportService reportService)
    {
      _reportService = reportService;
    }

    /// <summary>
    /// Criar novo laudo médico
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<MedicalReportResponseDto>> CreateReport(CreateMedicalReportDto dto)
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

        var report = await _reportService.CreateReportAsync(dto, user.Id);
        return CreatedAtAction(nameof(GetReport), new { id = report.Id }, report);
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
    /// Buscar laudo por ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<MedicalReportResponseDto>> GetReport(Guid id)
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

        var report = await _reportService.GetReportByIdAsync(id, user.Id);
        return report == null ? NotFound() : Ok(report);
      }
      catch (Exception ex)
      {
        return StatusCode(500, new { message = "Erro interno do servidor", detail = ex.Message });
      }
    }

    /// <summary>
    /// Listar laudos por paciente
    /// </summary>
    [HttpGet("patient/{patientId}")]
    public async Task<ActionResult<IEnumerable<MedicalReportResponseDto>>> GetReportsByPatient(Guid patientId)
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

        var reports = await _reportService.GetReportsByPatientIdAsync(patientId, user.Id);
        return Ok(reports);
      }
      catch (Exception ex)
      {
        return StatusCode(500, new { message = "Erro interno do servidor", detail = ex.Message });
      }
    }

    /// <summary>
    /// Listar todos os laudos do médico
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MedicalReportResponseDto>>> GetReports()
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

        var reports = await _reportService.GetReportsByUserIdAsync(user.Id);
        return Ok(reports);
      }
      catch (Exception ex)
      {
        return StatusCode(500, new { message = "Erro interno do servidor", detail = ex.Message });
      }
    }

    /// <summary>
    /// Listar laudos recentes
    /// </summary>
    [HttpGet("recent")]
    public async Task<ActionResult<IEnumerable<MedicalReportResponseDto>>> GetRecentReports([FromQuery] int count = 10)
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

        var reports = await _reportService.GetRecentReportsAsync(user.Id, count);
        return Ok(reports);
      }
      catch (Exception ex)
      {
        return StatusCode(500, new { message = "Erro interno do servidor", detail = ex.Message });
      }
    }

    /// <summary>
    /// Atualizar laudo
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<MedicalReportResponseDto>> UpdateReport(Guid id, CreateMedicalReportDto dto)
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

        var report = await _reportService.UpdateReportAsync(id, dto, user.Id);
        return Ok(report);
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
    /// Atualizar status do laudo
    /// </summary>
    [HttpPatch("{id}/status")]
    public async Task<ActionResult<MedicalReportResponseDto>> UpdateReportStatus(Guid id, [FromBody] ReportStatus status)
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

        var report = await _reportService.UpdateReportStatusAsync(id, status, user.Id);
        return Ok(report);
      }
      catch (InvalidOperationException ex)
      {
        return BadRequest(new { message = ex.Message });
      }
    }

    /// <summary>
    /// Excluir laudo (soft delete)
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteReport(Guid id)
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

        await _reportService.DeleteReportAsync(id, user.Id);
        return NoContent();
      }
      catch (InvalidOperationException ex)
      {
        return BadRequest(new { message = ex.Message });
      }
    }

    /// <summary>
    /// Gerar PDF do laudo
    /// </summary>
    [HttpGet("{id}/pdf")]
    public async Task<ActionResult> GenerateReportPdf(Guid id)
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

        var pdfBytes = await _reportService.GeneratePdfAsync(id, user.Id);
        return File(pdfBytes, "application/pdf", $"laudo-{id}.pdf");
      }
      catch (NotImplementedException)
      {
        return BadRequest(new { message = "Geração de PDF ainda não implementada" });
      }
      catch (InvalidOperationException ex)
      {
        return BadRequest(new { message = ex.Message });
      }
    }
  }
}