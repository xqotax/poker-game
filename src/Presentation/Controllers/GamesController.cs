using Application.Games.Commands.AcceptBetsOnRound;
using Application.Games.Commands.AddBribesOnRound;
using Application.Games.Commands.Create;
using Application.Games.Commands.StartNewRound;
using Application.Games.Queries.Get;
using Application.Games.Queries.GetAll;
using Application.Users.Queries.GetAll;
using AvtMedia.CleanArchitecture.DomainLayer.Extensions.Shared;
using AvtMedia.GeneralLibrary.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Presentation.Common;
using Presentation.Games.Extensions;
using Presentation.Games.ViewModels;
using Presentation.Users.Extensions;

namespace Presentation.Controllers;

[ApiController]
[Route("api/games")]
public sealed class GamesController(ISender _sender) : ControllerBase
{
	[HttpGet("{gameId}")]
	[ProducesResponseType(typeof(GameViewModel), 200)]
	[ProducesResponseType(typeof(Error), 400)]
	public async Task<IActionResult> GetById(
		[FromRoute] string gameId,
		CancellationToken cancellationToken)
	{
		var getGameByIdQuery = new GetGameByIdQuery(gameId.ToGuid());
		var getGameResult = await _sender.Send(getGameByIdQuery, cancellationToken);
		if (getGameResult.IsFailure)
			return BadRequest(getGameResult.Error);

		var game = getGameResult.Value;


		var getAllUsersQuery = new GetAllUsersQuery();
		var getAllUsersResult = await _sender.Send(getAllUsersQuery, cancellationToken);
		if (getAllUsersResult.IsFailure)
			return BadRequest(getAllUsersResult.Error);

		var memberIds = game.Members.Select(m => m.UserId).ToArray();

		var memberViewModels = getAllUsersResult.Value
			.Where(x => memberIds.Contains(x.Id))
			.Select(x =>
			{
				var member = game.Members.First(m => m.UserId == x.Id);
				return x.ToGameViewModel(member.OrderIndex, member.IsWinner);
			})
			.ToArray();

		var gameViewModel = game.ToViewModel(memberViewModels);

		return Ok(gameViewModel);
	}

	[HttpGet("all")]
	[ProducesResponseType(typeof(GamePreviewViewModel[]), 200)]
	[ProducesResponseType(typeof(Error), 400)]
	public async Task<IActionResult> GetAll(
		CancellationToken cancellationToken)
	{
		var getAllGamesQuery = new GetGamesQuery();

		var result = await _sender.Send(getAllGamesQuery, cancellationToken);

		if (result.IsFailure)
			return BadRequest(result.Error);

		var getAllUsersQuery = new GetAllUsersQuery();
		var getAllUsersResult = await _sender.Send(getAllUsersQuery, cancellationToken);
		if (getAllUsersResult.IsFailure)
			return BadRequest(getAllUsersResult.Error);

		var response = result.Value.Select(game =>
		{
			var memberViewModels = game.MemberIds
				.Select(m => getAllUsersResult.Value
					.FirstOrDefault(u => u.Id == m)?
					.ToStringDictionaryModel() ?? new StringDictionaryModel(m.ToString(), "Unknown User"))
				.ToArray();
			return game.ToViewModel(memberViewModels);
		}).ToArray();


		return Ok(response);
	}

	[HttpPost("{gameId}/start-new-round")]
	[ProducesResponseType(200)]
	[ProducesResponseType(typeof(Error), 400)]
	public async Task<IActionResult> StartNewRound(
	[FromRoute] string gameId,
	CancellationToken cancellationToken)
	{
		var command = new StartNewRoundCommand(gameId.ToGuid());

		var result = await _sender.Send(command, cancellationToken);

		return result.IsSuccess ? Ok() : BadRequest(result.Error);
	}

	[HttpPost("{gameId}/accept-bets")]
	[ProducesResponseType(200)]
	[ProducesResponseType(typeof(Error), 400)]
	public async Task<IActionResult> AcceptBets(
		[FromRoute] string gameId,
		[FromBody] AcceptBetOnRoundViewModel[] bets,
		CancellationToken cancellationToken)
	{
		var command = new AcceptBetsOnRoundCommand(
			gameId.ToGuid(),
			bets.ToDictionary(k => k.MemberId.ToGuid(), v => v.Amount));

		var result = await _sender.Send(command, cancellationToken);

		return result.IsSuccess ? Ok() : BadRequest(result.Error);
	}

	[HttpPost("{gameId}/add-bribes")]
	[ProducesResponseType(200)]
	[ProducesResponseType(typeof(Error), 400)]
	public async Task<IActionResult> AddBribes(
		[FromRoute] string gameId,
		[FromBody] AddBribeOnRoundViewModel[] bribes,
		CancellationToken cancellationToken)
	{
		var command = new AddBribesOnRoundCommand(
			gameId.ToGuid(),
			bribes.ToDictionary(k => k.MemberId.ToGuid(), v => v.Amount));

		var result = await _sender.Send(command, cancellationToken);

		return result.IsSuccess ? Ok() : BadRequest(result.Error);
	}

	[HttpPost("create")]
	[ProducesResponseType(typeof(string), 200)]
	[ProducesResponseType(typeof(Error), 400)]
	public async Task<IActionResult> Create(
		[FromBody] GameCreateViewModel model,
		CancellationToken cancellationToken)
	{
		var command = new CreateGameCommand(model.Name, model.Members.ToDictionary(k => k.MemberId.ToGuid(), v => v.OrderIndex));

		var result = await _sender.Send(command, cancellationToken);

		return result.IsSuccess ? Ok(result.Value.ToString()) : BadRequest(result.Error);
	}
}
