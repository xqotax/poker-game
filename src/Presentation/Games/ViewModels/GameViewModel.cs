using Domain.Games.Enums;

namespace Presentation.Games.ViewModels;

public sealed record GameViewModel(
	string Id,
	string Name,
	DateTime CreatedOnUtc,
	GameState State,
	GameMemberViewModel[] Members,
	RoundViewModel[] Rounds
);
