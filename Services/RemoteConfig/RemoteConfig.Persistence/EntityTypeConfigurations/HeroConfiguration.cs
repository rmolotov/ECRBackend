using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RemoteConfig.Core.Entities.Hero;

namespace RemoteConfig.Persistence.EntityTypeConfigurations;

public class HeroConfiguration : IEntityTypeConfiguration<Hero>
{
    public void Configure(EntityTypeBuilder<Hero> builder)
    {
        builder
            .HasKey(note => note.Id);
        builder
            .HasIndex(note => note.Id)
            .IsUnique();
        builder
            .Property(hero => hero.Capacity);
    }
}