using AvtMedia.CleanArchitecture.InfrastructureLayer.Extensions.Extensions;
using AvtMedia.CleanArchitecture.InfrastructureLayer.Extensions.Outbox;
using MediatR;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Infrastructure.BackgroundJobs;

[DisallowConcurrentExecution]
public class ProccessOutboxMessagesJob(
	ApplicationDbContext _dbContext,
	IPublisher _publisher,
	ILogger<ProccessOutboxMessagesJob> _logger) : IJob
{
	public Task Execute(IJobExecutionContext context)
	{
		return _dbContext.ExecuteWithDbLock(
			ApplicationDbContext.SchemaName,
			ct => OutboxProcessor.Process(_dbContext, _publisher, _logger, count: 1, ct),
			logger: null,
			context.CancellationToken
		);
	}
}
