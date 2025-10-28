using Domain.Users;

namespace Application.Users.Queries.Get;

public sealed record GetUserByIdQuery(Guid Id): IQuery<User>, ILoggingProperties;
