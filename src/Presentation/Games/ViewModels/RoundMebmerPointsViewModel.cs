namespace Presentation.Games.ViewModels;

public sealed record RoundMebmerPointsViewModel(
	Guid MemberId,
	uint? BetCount,
	uint BribeCount,
	int Before,
	int After,
	int Delta
);
