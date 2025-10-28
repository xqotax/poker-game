using Domain.Users;
using Domain.Users.ValueObjects;
using Infrastructure.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

internal class UsersConfiguration : IEntityTypeConfiguration<User>
{
	public void Configure(EntityTypeBuilder<User> builder)
	{
		builder.ToTable(TableNames.Users);

		builder.HasKey(x => x.Id);

		builder.Property(x => x.Username)
			.HasConversion(
				v => v.Value,
				v => UserName.Create(v).Value)
			.HasMaxLength(UserName.MaxLength)
			.IsRequired();

		builder.HasIndex(x => x.Username)
			.IsUnique();
	}
}
