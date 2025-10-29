using Application.Users.Commands.Save;
using Application.Users.Queries.Get;
using Application.Users.Queries.GetAll;
using AvtMedia.CleanArchitecture.DomainLayer.Extensions.Shared;
using AvtMedia.GeneralLibrary.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Presentation.Common;
using Presentation.Users.Extensions;
using Presentation.Users.ViewModels;

namespace Presentation.Controllers;

[ApiController]
[Route("api/users")]
public sealed class UsersController(ISender _sender) : ControllerBase
{
	[HttpPost("create")]
	[ProducesResponseType(typeof(string), 200)]
	[ProducesResponseType(typeof(Error), 400)]
	public async Task<IActionResult> Create(
		[FromBody] SingleNameModel body,
		CancellationToken cancellationToken)
	{
		var createUserCommand = new SaveUserCommand(Id: null, body.Name ?? string.Empty);

		var result = await _sender.Send(createUserCommand, cancellationToken);

		return result.IsSuccess ? Ok(result.Value.ToString()) : BadRequest(result.Error);
	}

	[HttpPost("{userId}/update")]
	[ProducesResponseType(200)]
	[ProducesResponseType(typeof(Error), 400)]
	public async Task<IActionResult> Create(
		[FromRoute] string userId,
		[FromBody] SingleNameModel body,
		CancellationToken cancellationToken)
	{
		var createUserCommand = new SaveUserCommand(userId.ToGuid(), body.Name ?? string.Empty);

		var result = await _sender.Send(createUserCommand, cancellationToken);

		return result.IsSuccess ? Ok() : BadRequest(result.Error);
	}

	[HttpGet("{userId}")]
	[ProducesResponseType(typeof(UserViewModel), 200)]
	[ProducesResponseType(typeof(Error), 400)]
	public async Task<IActionResult> GetById(
		[FromRoute] string userId,
		CancellationToken cancellationToken)
	{
		var getUserByIdQuery = new GetUserByIdQuery(userId.ToGuid());

		var result = await _sender.Send(getUserByIdQuery, cancellationToken);

		if (result.IsFailure)
			return BadRequest(result.Error);

		var userViewModel = result.Value.ToViewModel();

		return Ok(userViewModel);
	}

	[HttpGet("all")]
	[ProducesResponseType(typeof(UserViewModel[]), 200)]
	[ProducesResponseType(typeof(Error), 400)]
	public async Task<IActionResult> GetAll(
		CancellationToken cancellationToken)
	{
		var getAllUsersQuery = new GetAllUsersQuery();

		var result = await _sender.Send(getAllUsersQuery, cancellationToken);

		if (result.IsFailure)
			return BadRequest(result.Error);

		var userViewModels = result.Value
			.Select(x => x.ToViewModel())
			.ToArray();

		return Ok(userViewModels);
	}
}
