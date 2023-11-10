using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zev.Core.Domain.Examples;

namespace Zev.Core.Infrastructure.Persistence.Configurations.Examples;

public class ExampleModelConfiguration : IEntityTypeConfiguration<ExampleModel>
{
    public void Configure(EntityTypeBuilder<ExampleModel> builder)
    {
        builder.HasKey(x => x.Id);
    }
}