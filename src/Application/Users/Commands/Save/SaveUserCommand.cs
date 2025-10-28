namespace Application.Users.Commands.Save;

public sealed record SaveUserCommand(Guid? Id, string Username): ICommand<Guid>, ILoggingProperties;
