using Domain.Games.Enums;

namespace Presentation.Games.ViewModels;

public sealed record RoundViewModel(
	string Id,
	GameRoundType Type,
	uint GeneralNumber,
	uint TypeNumber
);
