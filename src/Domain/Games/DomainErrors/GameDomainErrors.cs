using AvtMedia.CleanArchitecture.DomainLayer.Extensions.Shared;

namespace Domain.Games.DomainErrors;

public static class GameDomainErrors
{
	public static class Game
	{
		public static readonly Error InvalidUserCount = new("Game.InvalidUserCount");
		public static readonly Error DuplicateMembers = new("Game.DuplicateMembers");
		public static readonly Error InvalidMemberOrder = new("Game.InvalidMemberOrder");
		public static readonly Error MemberNotFound = new("Game.MemberNotFound");
		public static readonly Error DuplicateRound = new("Game.DuplicateRound");
		public static readonly Error FailedToDetermineWinner = new("Game.FailedToDetermineWinner");
		public static readonly Error AlreadyFinished = new("Game.AlreadyFinished");
		public static readonly Error NoActiveRound = new("Game.NoActiveRound");
	}

	public static class GameName
	{
		public static readonly Error Empty = new("GameName.Empty");
		public static readonly Error TooLong = new("GameName.TooLong");
		public static readonly Error TooShort = new("GameName.TooShort");
	}

	public static class GameRound
	{
		public static readonly Error InvalidGeneralNumber = new("GameRound.InvalidGeneralNumber");
		public static readonly Error InvalidTypeNumber = new("GameRound.InvalidTypeNumber");
		public static readonly Error InvalidType = new("GameRound.InvalidType");
		public static readonly Error NoBetsProvided = new("GameRound.NoBetsProvided");
		public static readonly Error BetsAlreadyAccepted = new("GameRound.BetsAlreadyAccepted");
		public static readonly Error InvalidBetForRound = new("GameRound.InvalidBetForRound");
		public static readonly Error DuplicateBets = new("GameRound.DuplicateBets");
		public static readonly Error DuplicateBribes = new("GameRound.DuplicateBribes");
		public static readonly Error NoBribesProvided = new("GameRound.NoBribesProvided");
		public static readonly Error BribesAlreadyAccepted = new("GameRound.BribesAlreadyAccepted");
		public static readonly Error BribeBetCountMismatch = new("GameRound.BribeBetCountMismatch");
		public static readonly Error FailedToDetirminePoints = new("GameRound.FailedToDetirminePoints");
		public static readonly Error MemberNotInRound = new("GameRound.MemberNotInRound");
		public static readonly Error RoundNotFinished = new("GameRound.RoundNotFinished");
	}
}
