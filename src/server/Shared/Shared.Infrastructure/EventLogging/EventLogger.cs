﻿using FluentPOS.Shared.Core.Domain;
using FluentPOS.Shared.Core.EventLogging;
using FluentPOS.Shared.Core.Interfaces;
using FluentPOS.Shared.Core.Interfaces.Services.Identity;
using System.Threading.Tasks;
using FluentPOS.Shared.Core.Interfaces.Serialization;

namespace FluentPOS.Shared.Infrastructure.EventLogging
{
    internal class EventLogger : IEventLogger
    {
        private readonly ICurrentUser _user;
        private readonly IApplicationDbContext _context;
        private readonly IJsonSerializer _jsonSerializer;

        public EventLogger(ICurrentUser user, IApplicationDbContext context, IJsonSerializer jsonSerializer)
        {
            _user = user;
            _context = context;
            _jsonSerializer = jsonSerializer;
        }

        public async Task Save<T>(T @event) where T : Event
        {
            var serializedData = _jsonSerializer.Serialize(@event);

            var thisEvent = new EventLog(
                @event,
                serializedData,
                _user.Name ?? _user.GetUserEmail());
            await _context.EventLogs.AddAsync(thisEvent);
            await _context.SaveChangesAsync(default);
        }
    }
}