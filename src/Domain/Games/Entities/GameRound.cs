using AvtMedia.CleanArchitecture.DomainLayer.Extensions.Primitives;
using AvtMedia.CleanArchitecture.DomainLayer.Extensions.Shared;
using Domain.Games.DomainErrors;
using Domain.Games.Enums;

namespace Domain.Games.Entities;

public sealed partial class GameRound : Entity<Guid>
{
	public GameRoundType Type { get; private set; }
	public uint GeneralNumber { get; private set; }

	private readonly HashSet<GameRoundBribe> _bribes = [];
	public ICollection<GameRoundBribe> Bribes => [.. _bribes];


	private readonly HashSet<GameRoundBet> _bets = [];
	public ICollection<GameRoundBet> Bets => [.. _bets];

	public static Result<GameRound> Create(GameRoundType type, uint generalNumber)
	{
		if (generalNumber is 0)
			return Result.Failure<GameRound>(GameDomainErrors.GameRound.InvalidGeneralNumber);

		if (type is GameRoundType.None)
			return Result.Failure<GameRound>(GameDomainErrors.GameRound.InvalidType);

		var round = new GameRound(type, generalNumber);

		return round;
	}

	private static int MaxBetsCount(int membersCount) => membersCount switch
	{
		3 => 10,
		4 => 9,
		5 => 10,
		_ => throw new ArgumentOutOfRangeException(nameof(membersCount), "Unsupported members count")
	};

	private static int[] RegularNumbers(int membersCount) => membersCount switch
	{
		3 => [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 10, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1],
		4 => [1, 2, 3, 4, 5, 6, 7, 8, 9, 9, 9, 9, 8, 7, 6, 5, 4, 3, 2, 1],
		5 => [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 10, 10, 10, 10, 10, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1],
		_ => throw new ArgumentOutOfRangeException(nameof(membersCount), "Unsupported members count")
	};

	public static IReadOnlyCollection<(GameRoundType Type, uint Count)> RoundSequence(int membersCount) =>
	[
		(GameRoundType.Regular, (uint)RegularNumbers(membersCount).Length),
		(GameRoundType.Dark, (uint)membersCount),
		(GameRoundType.Meager, (uint)membersCount),
		(GameRoundType.Trumpless, (uint)membersCount),
		(GameRoundType.Golden, (uint)membersCount),
		(GameRoundType.Forehead, (uint)membersCount)
	];

	public string GetDisplayName(int membersCount)
	{
		uint roundIndex = GeneralNumber;
		uint accumulated = 0;

		foreach (var (type, count) in RoundSequence(membersCount))
		{
			if (roundIndex <= accumulated + count)
			{
				if (type == GameRoundType.Regular)
				{
					int regularIdx = (int)(roundIndex - accumulated - 1);
					if (regularIdx >= 0 && regularIdx < RegularNumbers(membersCount).Length)
						return RegularNumbers(membersCount)[regularIdx].ToString();
					else
						return roundIndex.ToString();
				}
				else
				{
					return type.ToString();
				}
			}
			accumulated += count;
		}

		return GeneralNumber.ToString();
	}

	public Result AcceptBets(GameRoundBet[] bets, int membersCount)
	{
		if (Type is GameRoundType.Meager)
			return Result.Failure(GameDomainErrors.GameRound.CanAcceptBetsAsMeager);

		if (bets.Length == 0)
			return Result.Failure(GameDomainErrors.GameRound.NoBetsProvided);

		if (_bets.Count > 0)
			return Result.Failure(GameDomainErrors.GameRound.BetsAlreadyAccepted);

		var someBetsHasntAmount = bets.Any(b => b.Amount is null);

		if (someBetsHasntAmount && membersCount == 3)
			return Result.Failure(GameDomainErrors.GameRound.InvalidBetForRound);

		var duplicateBets = bets.Length != bets.Select(x => x.Id).Distinct().Count();

		if (duplicateBets)
			return Result.Failure(GameDomainErrors.GameRound.DuplicateBets);

		if (Type != GameRoundType.Forehead)
		{
			int sum = bets.Sum(b => b.Amount ?? 0);

			if (Type == GameRoundType.Regular)
			{
				int idx = (int)(GeneralNumber - 1);
				if (idx < 0 || idx >= RegularNumbers(membersCount).Length || sum == RegularNumbers(membersCount)[idx])
					return Result.Failure(GameDomainErrors.GameRound.InvalidBetForRound);
			}
			else
			{
				if (sum == MaxBetsCount(membersCount))
					return Result.Failure(GameDomainErrors.GameRound.InvalidBetForRound);
			}
		}


		_bets.UnionWith(bets);

		return Result.Success();
	}

	internal Result AcceptBribes(GameRoundBribe[] bribes, int membersCount)
	{
		if (bribes.Length == 0)
			return Result.Failure(GameDomainErrors.GameRound.NoBribesProvided);

		if (_bribes.Count > 0)
			return Result.Failure(GameDomainErrors.GameRound.BribesAlreadyAccepted);

		if (bribes.Length != _bets.Count && Type is not GameRoundType.Meager)
			return Result.Failure(GameDomainErrors.GameRound.BribeBetCountMismatch);

		var duplicateBribes = bribes.Length != bribes.Select(x => x.Id).Distinct().Count();

		if (duplicateBribes)
			return Result.Failure(GameDomainErrors.GameRound.DuplicateBribes);

		_bribes.UnionWith(bribes);

		int sum = bribes.Sum(b => b.Amount);

		if (Type == GameRoundType.Forehead)
		{
			if (sum != 1)
				return Result.Failure(GameDomainErrors.GameRound.InvalidBribeForRound);
		}
		else if (Type == GameRoundType.Regular)
		{
			int idx = (int)(GeneralNumber - 1);
			if (idx < 0 || idx >= RegularNumbers(membersCount).Length || sum != RegularNumbers(membersCount)[idx])
				return Result.Failure(GameDomainErrors.GameRound.InvalidBribeForRound);
		}
		else
		{
			if (sum != MaxBetsCount(membersCount))
				return Result.Failure(GameDomainErrors.GameRound.InvalidBribeForRound);
		}

		return Result.Success();
	}

	public Result<int> GetPoints(GameMember gameMember)
	{
		if (_bribes.Count == 0 || _bets.Count == 0 && Type is not GameRoundType.Meager)
			return Result.Failure<int>(GameDomainErrors.GameRound.RoundNotFinished);

		var bet = _bets.FirstOrDefault(b => b.MemberId == gameMember.UserId);
		var bribe = _bribes.FirstOrDefault(b => b.MemberId == gameMember.UserId);

		if (bribe is null)
			return Result.Failure<int>(GameDomainErrors.GameRound.MemberNotInRound);

		if (bet?.Amount is null && Type is not GameRoundType.Meager)
			return 0;

		if (Type is GameRoundType.Meager)
		{
			if (bribe.Amount == 0)
				return Result.Success(5);

			return Result.Success(-10 * bribe.Amount);
		}

		if (bet is null)
			return 0;

		var coef = Type switch
		{
			GameRoundType.Forehead => 10,
			GameRoundType.Golden => 2,
			_ => 1
		};


		var passSuccess = bet.Amount == bribe.Amount && bribe.Amount == 0;
		var betSuccess = bet.Amount == bribe.Amount && bribe.Amount != 0;
		var overBet = bribe.Amount > bet.Amount;
		var underBet = bribe.Amount < bet.Amount;

		if (passSuccess)
			return Result.Success(5 * coef);
		else if (betSuccess)
			return Result.Success(10 * coef * bribe.Amount);
		else if (overBet)
			return Result.Success(bribe.Amount * coef);
		else if (underBet)
			return Result.Success(((bet.Amount - bribe.Amount) * -10 * coef) ?? 0);
		else
			return Result.Failure<int>(GameDomainErrors.GameRound.FailedToDetirminePoints);
	}

	private GameRound(GameRoundType type, uint generalNumber)
	{
		Id = Guid.CreateVersion7();
		Type = type;
		GeneralNumber = generalNumber;
	}
}
