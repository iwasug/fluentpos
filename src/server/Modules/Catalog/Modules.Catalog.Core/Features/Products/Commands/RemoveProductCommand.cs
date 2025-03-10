﻿using FluentPOS.Shared.Core.Wrapper;
using MediatR;
using System;

namespace FluentPOS.Modules.Catalog.Core.Features.Products.Commands
{
    public class RemoveProductCommand : IRequest<Result<Guid>>
    {
        public Guid Id { get; set; }

        public RemoveProductCommand(Guid productId)
        {
            Id = productId;
        }
    }
}