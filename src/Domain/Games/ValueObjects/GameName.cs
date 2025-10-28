using AvtMedia.CleanArchitecture.DomainLayer.Extensions.Primitives;
using AvtMedia.CleanArchitecture.DomainLayer.Extensions.Shared;
using Domain.Games.DomainErrors;

namespace Domain.Games.ValueObjects;

public sealed partial class GameName : ValueObject
{
	public string Value { get; private set; }

	public const short MinLength = 5;
	public const short MaxLength = 200;

	internal GameName(string value) => Value = value;

	public static Result<GameName> Create(string name)
	{
		if (string.IsNullOrWhiteSpace(name))
			return Result.Failure<GameName>(GameDomainErrors.GameName.Empty);

		if (name.Length > MaxLength)
			return Result.Failure<GameName>(GameDomainErrors.GameName.TooLong);

		if (name.Length < MinLength)
			return Result.Failure<GameName>(GameDomainErrors.GameName.TooShort);

		return new GameName(name);
	}

	public override IEnumerable<object> GetAtomicValues()
	{
		yield return Value;
	}

	public static explicit operator string(GameName value) => value.Value;
}
