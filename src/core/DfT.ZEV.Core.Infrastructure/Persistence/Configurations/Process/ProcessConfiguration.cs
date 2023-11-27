using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DfT.ZEV.Core.Infrastructure.Persistence.Configurations.Process;

public sealed class ProcessConfiguration : IEntityTypeConfiguration<Domain.Processes.Models.Process>
{
    public void Configure(EntityTypeBuilder<Domain.Processes.Models.Process> builder)
    {
        builder.HasKey(x => x.Id);
    }
}