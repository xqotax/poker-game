using Domain.Games.Enums;
using Presentation.Common;

namespace Presentation.Games.ViewModels;

public sealed record GamePreviewViewModel(
	string Id,
	string Name,
	DateTime CreatedOnUtc,
	GameState State,
	StringDictionaryModel[] Members
);
