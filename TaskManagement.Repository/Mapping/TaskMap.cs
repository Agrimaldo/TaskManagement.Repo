
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using enums = TaskManagement.Domain.Util.Enumerators;
using domain = TaskManagement.Domain.Entities;

namespace TaskManagement.Repository.Mapping
{
    public class TaskMap : IEntityTypeConfiguration<domain.Task>
    {
        public void Configure(EntityTypeBuilder<domain.Task> builder)
        {
            builder.ToTable("Tb_Task");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).HasColumnName("Id").ValueGeneratedOnAdd();
            builder.Property(e => e.Description).HasColumnName("Description").HasMaxLength(100);
            builder.Property(e => e.Status).HasColumnName("Status").HasConversion(new EnumToStringConverter<enums.TaskStatus>()).HasMaxLength(20);
            builder.Property(e => e.Date).HasColumnName("Date");
            builder.Property(e => e.CreatedAt).HasColumnName("CreatedAt");
            builder.Property(e => e.UpdatedAt).HasColumnName("UpdatedAt");
        }
    }
}
