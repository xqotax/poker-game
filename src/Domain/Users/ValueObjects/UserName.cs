using AvtMedia.CleanArchitecture.DomainLayer.Extensions.Primitives;
using AvtMedia.CleanArchitecture.DomainLayer.Extensions.Shared;

namespace Domain.Users.ValueObjects;

public sealed partial class UserName :  ValueObject
{
	public string Value { get; private set; }

	public const short MinLength = 3;
	public const short MaxLength = 200;

	internal UserName(string value) => Value = value;

	public static Result<UserName> Create(string name)
	{
		if (string.IsNullOrWhiteSpace(name))
			return Result.Failure<UserName>(DomainErrors.UserDomainErrors.UserName.Empty);

		if (name.Length > MaxLength)
			return Result.Failure<UserName>(DomainErrors.UserDomainErrors.UserName.TooLong);

		if (name.Length < MinLength)
			return Result.Failure<UserName>(DomainErrors.UserDomainErrors.UserName.TooShort);

		return new UserName(name);
	}

	public override IEnumerable<object> GetAtomicValues()
	{
		yield return Value;
	}

	public static explicit operator string(UserName value) => value.Value;
}
