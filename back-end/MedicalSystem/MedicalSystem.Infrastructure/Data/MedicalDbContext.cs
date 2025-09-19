using Microsoft.EntityFrameworkCore;
using MedicalSystem.Domain.Entities;
using MedicalSystem.Domain.ValueObjects;
using MedicalSystem.Domain.Enums;
using System.Text.Json;

namespace MedicalSystem.Infrastructure.Data
{
  public class MedicalDbContext : DbContext
  {
    public MedicalDbContext(DbContextOptions<MedicalDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<MedicalReport> MedicalReports { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      ConfigureUserEntity(modelBuilder);
      ConfigurePatientEntity(modelBuilder);
      ConfigureMedicalReportEntity(modelBuilder);
    }

    private void ConfigureUserEntity(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<User>(entity =>
      {
        // Primary Key
        entity.HasKey(e => e.Id);

        // Properties
        entity.Property(e => e.Email)
            .IsRequired()
            .HasMaxLength(255);

        entity.Property(e => e.PasswordHash)
            .IsRequired()
            .HasMaxLength(500);

        entity.Property(e => e.FullName)
            .IsRequired()
            .HasMaxLength(255);

        entity.Property(e => e.Crm)
            .IsRequired()
            .HasMaxLength(20);

        entity.Property(e => e.DigitalSignature)
            .HasColumnType("text");

        entity.Property(e => e.LogoUrl)
            .HasMaxLength(500);

        entity.Property(e => e.RefreshToken)
            .HasMaxLength(500);

        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        // Indexes
        entity.HasIndex(e => e.Email)
            .IsUnique()
            .HasDatabaseName("IX_Users_Email");

        entity.HasIndex(e => e.Crm)
            .IsUnique()
            .HasDatabaseName("IX_Users_Crm");

        // Relationships
        entity.HasMany(e => e.Patients)
            .WithOne(e => e.User)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasMany(e => e.MedicalReports)
            .WithOne(e => e.User)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Restrict);
      });
    }

    private void ConfigurePatientEntity(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<Patient>(entity =>
      {
        // Primary Key
        entity.HasKey(e => e.Id);

        // Properties
        entity.Property(e => e.FullName)
            .IsRequired()
            .HasMaxLength(255);

        entity.Property(e => e.Cpf)
            .IsRequired()
            .HasMaxLength(11);

        entity.Property(e => e.Gender)
            .IsRequired()
            .HasMaxLength(10);

        entity.Property(e => e.Email)
            .HasMaxLength(255);

        entity.Property(e => e.Phone)
            .HasMaxLength(20);

        entity.Property(e => e.HealthPlan)
            .HasMaxLength(255);

        entity.Property(e => e.Allergies)
            .HasColumnType("text");

        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        // Address as owned entity (JSON column)
        entity.OwnsOne(e => e.Address, address =>
        {
          address.Property(a => a.Street).HasMaxLength(255);
          address.Property(a => a.Number).HasMaxLength(10);
          address.Property(a => a.Complement).HasMaxLength(100);
          address.Property(a => a.Neighborhood).HasMaxLength(100);
          address.Property(a => a.City).HasMaxLength(100);
          address.Property(a => a.State).HasMaxLength(50);
          address.Property(a => a.ZipCode).HasMaxLength(10);
        });

        // Indexes
        entity.HasIndex(e => e.Cpf)
            .IsUnique()
            .HasDatabaseName("IX_Patients_Cpf");

        entity.HasIndex(e => e.FullName)
            .HasDatabaseName("IX_Patients_FullName");

        entity.HasIndex(e => e.UserId)
            .HasDatabaseName("IX_Patients_UserId");

        entity.HasIndex(e => new { e.UserId, e.IsDeleted })
            .HasDatabaseName("IX_Patients_UserId_IsDeleted");

        // Relationships
        entity.HasMany(e => e.MedicalReports)
            .WithOne(e => e.Patient)
            .HasForeignKey(e => e.PatientId)
            .OnDelete(DeleteBehavior.Restrict);
      });
    }

    private void ConfigureMedicalReportEntity(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<MedicalReport>(entity =>
      {
        // Primary Key
        entity.HasKey(e => e.Id);

        // Properties
        entity.Property(e => e.ReportType)
            .HasConversion<int>()
            .IsRequired();

        entity.Property(e => e.PathologyDuration)
            .HasMaxLength(255);

        entity.Property(e => e.Diagnosis)
            .IsRequired()
            .HasColumnType("text");

        entity.Property(e => e.TreatmentPerformed)
            .HasColumnType("text");

        entity.Property(e => e.TreatmentImageUrl)
            .HasMaxLength(500);

        entity.Property(e => e.Prescription)
            .HasColumnType("text");

        entity.Property(e => e.DiseaseDisabilities)
            .HasColumnType("text");

        entity.Property(e => e.DiseaseDuration)
            .HasMaxLength(255);

        entity.Property(e => e.Cid10)
            .HasMaxLength(10);

        entity.Property(e => e.Prognosis)
            .HasColumnType("text");

        entity.Property(e => e.PrognosisImageUrl)
            .HasMaxLength(500);

        entity.Property(e => e.Status)
            .HasConversion<int>();

        entity.Property(e => e.PdfUrl)
            .HasMaxLength(500);

        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        // Indexes
        entity.HasIndex(e => e.PatientId)
            .HasDatabaseName("IX_MedicalReports_PatientId");

        entity.HasIndex(e => e.UserId)
            .HasDatabaseName("IX_MedicalReports_UserId");

        entity.HasIndex(e => e.ConsultationDate)
            .HasDatabaseName("IX_MedicalReports_ConsultationDate");

        entity.HasIndex(e => e.Status)
            .HasDatabaseName("IX_MedicalReports_Status");

        entity.HasIndex(e => new { e.UserId, e.IsDeleted })
            .HasDatabaseName("IX_MedicalReports_UserId_IsDeleted");

        entity.HasIndex(e => new { e.PatientId, e.IsDeleted })
            .HasDatabaseName("IX_MedicalReports_PatientId_IsDeleted");
      });
    }

    public override int SaveChanges()
    {
      ConvertDatesToUtc();
      return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
      ConvertDatesToUtc();
      return await base.SaveChangesAsync(cancellationToken);
    }

    private void ConvertDatesToUtc()
    {
      foreach (var entry in ChangeTracker.Entries())
      {
        foreach (var property in entry.Properties)
        {
          if (property.CurrentValue is DateTime dateTime && dateTime.Kind == DateTimeKind.Unspecified)
          {
            property.CurrentValue = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
          }
        }
      }
    }
  }
}