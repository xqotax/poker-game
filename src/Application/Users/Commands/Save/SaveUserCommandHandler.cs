using Domain.Common;
using Domain.Users;
using Domain.Users.DomainErrors;
using Domain.Users.Repositories;
using Domain.Users.ValueObjects;

namespace Application.Users.Commands.Save;

public sealed class SaveUserCommandHandler(
	IUsersRepository _usersRepository,
	IUnitOfWork _unitOfWork) : ICommandHandler<SaveUserCommand, Guid>
{
	public async Task<Result<Guid>> Handle(SaveUserCommand request, CancellationToken cancellationToken)
	{
		var userId = request.Id;

		var createUserNameResult = UserName.Create(request.Username);

		if (createUserNameResult.IsFailure)
			return Result.Failure<Guid>(createUserNameResult.Error);

		var nameAlreadyExists = await _usersRepository.NameAlreadyExist(
			createUserNameResult.Value,
			exceptId: request.Id,
			cancellationToken);

		if (nameAlreadyExists)
			return Result.Failure<Guid>(UserDomainErrors.User.AlreadyExistWithGivenName);

		if (request.Id.HasValue)
		{
			var user = await _usersRepository.GetById(request.Id.Value, cancellationToken);

			if (user is null)
				return Result.Failure<Guid>(UserDomainErrors.User.NotFound);

			var updateResult = user.Update(createUserNameResult.Value);

			if (updateResult.IsFailure)
				return Result.Failure<Guid>(updateResult.Error);
		}
		else
		{
			var newUser = User.Create(createUserNameResult.Value);

			if (newUser.IsFailure)
				return Result.Failure<Guid>(newUser.Error);

			await _usersRepository.Add(newUser.Value, cancellationToken);

			userId = newUser.Value.Id;
		}

		await _unitOfWork.SaveChangesAsync(cancellationToken);

		return userId!;
	}
}
