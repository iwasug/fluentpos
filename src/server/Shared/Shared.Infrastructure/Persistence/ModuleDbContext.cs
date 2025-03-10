﻿using FluentPOS.Shared.Core.Domain;
using FluentPOS.Shared.Core.EventLogging;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentPOS.Shared.Core.Contracts;
using FluentPOS.Shared.Infrastructure.Extensions;
using Microsoft.Extensions.Options;
using FluentPOS.Shared.Core.Settings;

namespace FluentPOS.Shared.Infrastructure.Persistence
{
    public abstract class ModuleDbContext : DbContext
    {
        private readonly IMediator _mediator;
        private readonly IEventLogger _eventLogger;
        private readonly PersistenceSettings _persistenceOptions;

        protected abstract string Schema { get; }

        protected ModuleDbContext(
            DbContextOptions options,
            IMediator mediator,
            IEventLogger eventLogger,
            IOptions<PersistenceSettings> persistenceOptions) : base(options)
        {
            _mediator = mediator;
            _eventLogger = eventLogger;
            _persistenceOptions = persistenceOptions.Value;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (!string.IsNullOrWhiteSpace(Schema))
            {
                modelBuilder.HasDefaultSchema(Schema);
            }
            modelBuilder.Ignore<Event>();
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
            modelBuilder.ApplyModuleConfiguration(_persistenceOptions);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var domainEntities = this.ChangeTracker
                .Entries<IBaseEntity>()
                .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any());

            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.DomainEvents)
                .ToList();

            domainEntities.ToList()
                .ForEach(entity => entity.Entity.ClearDomainEvents());

            var tasks = domainEvents
                .Select(async (domainEvent) =>
                {
                    await _eventLogger.Save(domainEvent);
                    await _mediator.Publish(domainEvent, cancellationToken);
                });
            await Task.WhenAll(tasks);
            return (await base.SaveChangesAsync(true, cancellationToken));
        }
    }
}