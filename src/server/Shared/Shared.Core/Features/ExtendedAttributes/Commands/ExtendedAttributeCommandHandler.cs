﻿using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentPOS.Shared.Core.Constants;
using FluentPOS.Shared.Core.Contracts;
using FluentPOS.Shared.Core.Domain;
using FluentPOS.Shared.Core.Exceptions;
using FluentPOS.Shared.Core.Features.ExtendedAttributes.Events;
using FluentPOS.Shared.Core.Interfaces;
using FluentPOS.Shared.Core.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Localization;

namespace FluentPOS.Shared.Core.Features.ExtendedAttributes.Commands
{
    public class ExtendedAttributeCommandHandler
    {
        // for localization
    }

    public class ExtendedAttributeCommandHandler<TEntityId, TEntity, TExtendedAttribute> :
        IRequestHandler<RemoveExtendedAttributeCommand<TEntityId, TEntity>, Result<Guid>>,
        IRequestHandler<AddExtendedAttributeCommand<TEntityId, TEntity>, Result<Guid>>,
        IRequestHandler<UpdateExtendedAttributeCommand<TEntityId, TEntity>, Result<Guid>>
            where TEntity : class, IEntity<TEntityId>
            where TExtendedAttribute : ExtendedAttribute<TEntityId, TEntity>
    {
        private readonly IDistributedCache _cache;
        private readonly IExtendedAttributeDbContext<TEntityId, TEntity, TExtendedAttribute> _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<ExtendedAttributeCommandHandler> _localizer;

        public ExtendedAttributeCommandHandler(
            IExtendedAttributeDbContext<TEntityId, TEntity, TExtendedAttribute> context,
            IMapper mapper,
            IStringLocalizer<ExtendedAttributeCommandHandler> localizer,
            IDistributedCache cache)
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
            _cache = cache;
        }

        public async Task<Result<Guid>> Handle(AddExtendedAttributeCommand<TEntityId, TEntity> command, CancellationToken cancellationToken)
        {
            var entity = await _context.Entities.AsNoTracking().FirstOrDefaultAsync(e => e.Id.Equals(command.EntityId), cancellationToken);
            if (entity == null) throw new CustomException(string.Format(_localizer["{0} Not Found"], typeof(TEntity).Name), statusCode: HttpStatusCode.NotFound);

            var isKeyUsed = await _context.ExtendedAttributes.AsNoTracking()
                .AnyAsync(ea => ea.EntityId.Equals(command.EntityId) && ea.Key.Equals(command.Key), cancellationToken);
            if (isKeyUsed) throw new CustomException(string.Format(_localizer["This {0} Key is Already Used For This Entity"], typeof(TEntity).Name), statusCode: HttpStatusCode.NotFound);

            var extendedAttribute = _mapper.Map<TExtendedAttribute>(command);
            extendedAttribute.AddDomainEvent(new ExtendedAttributeAddedEvent<TEntityId, TEntity>(extendedAttribute.Id,
                extendedAttribute.EntityId, extendedAttribute.Type, extendedAttribute.Key, extendedAttribute.Decimal,
                extendedAttribute.Text, extendedAttribute.DateTime, extendedAttribute.Json, extendedAttribute.Boolean, extendedAttribute.Integer,
                extendedAttribute.ExternalId, extendedAttribute.Group, extendedAttribute.Description,
                extendedAttribute.IsActive));
            await _context.ExtendedAttributes.AddAsync(extendedAttribute, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return await Result<Guid>.SuccessAsync(extendedAttribute.Id, string.Format(_localizer["{0} Extended Attribute Saved"], typeof(TEntity).Name));
        }

        public async Task<Result<Guid>> Handle(RemoveExtendedAttributeCommand<TEntityId, TEntity> command, CancellationToken cancellationToken)
        {
            var extendedAttribute = await _context.ExtendedAttributes.FirstOrDefaultAsync(ea => ea.Id == command.Id, cancellationToken);
            if (extendedAttribute == null) throw new CustomException(string.Format(_localizer["{0} Extended Attribute Not Found"], typeof(TEntity).Name), statusCode: HttpStatusCode.NotFound);

            _context.ExtendedAttributes.Remove(extendedAttribute);
            extendedAttribute.AddDomainEvent(new ExtendedAttributeRemovedEvent<TEntity>(command.Id));
            await _context.SaveChangesAsync(cancellationToken);
            await _cache.RemoveAsync(CacheKeys.GetExtendedAttributeByIdCacheKey(typeof(TEntity).Name, command.Id), cancellationToken);
            return await Result<Guid>.SuccessAsync(extendedAttribute.Id, string.Format(_localizer["{0} Extended Attribute Deleted"], typeof(TEntity).Name));
        }

        public async Task<Result<Guid>> Handle(UpdateExtendedAttributeCommand<TEntityId, TEntity> command, CancellationToken cancellationToken)
        {
            var extendedAttribute = await _context.ExtendedAttributes.Where(ea => ea.Id.Equals(command.Id)).AsNoTracking().FirstOrDefaultAsync(cancellationToken);
            if (extendedAttribute == null) throw new CustomException(string.Format(_localizer["{0} Extended Attribute Not Found!"], typeof(TEntity).Name), statusCode: HttpStatusCode.NotFound);

            if (!extendedAttribute.EntityId.Equals(command.EntityId)) throw new CustomException(string.Format(_localizer["{0} Not Found"], typeof(TEntity).Name), statusCode: HttpStatusCode.NotFound);

            var isKeyUsed = await _context.ExtendedAttributes.AsNoTracking()
                .AnyAsync(ea => ea.Id != extendedAttribute.Id && ea.EntityId.Equals(command.EntityId) && ea.Key.Equals(command.Key), cancellationToken);
            if (isKeyUsed) throw new CustomException(string.Format(_localizer["This {0} Key Is Already Used For This Entity"], typeof(TEntity).Name), statusCode: HttpStatusCode.NotFound);

            extendedAttribute = _mapper.Map<TExtendedAttribute>(command);
            extendedAttribute.AddDomainEvent(new ExtendedAttributeUpdatedEvent<TEntityId, TEntity>(extendedAttribute.Id,
                extendedAttribute.EntityId, extendedAttribute.Type, extendedAttribute.Key, extendedAttribute.Decimal,
                extendedAttribute.Text, extendedAttribute.DateTime, extendedAttribute.Json, extendedAttribute.Boolean, extendedAttribute.Integer,
                extendedAttribute.ExternalId, extendedAttribute.Group, extendedAttribute.Description,
                extendedAttribute.IsActive));
            _context.ExtendedAttributes.Update(extendedAttribute);
            await _context.SaveChangesAsync(cancellationToken);
            await _cache.RemoveAsync(CacheKeys.GetExtendedAttributeByIdCacheKey(typeof(TEntity).Name, command.Id), cancellationToken);
            return await Result<Guid>.SuccessAsync(extendedAttribute.Id, string.Format(_localizer["{0} Extended Attribute Updated"], typeof(TEntity).Name));
        }
    }
}