﻿using System;
using FluentPOS.Modules.Catalog.Core.Entities;
using FluentPOS.Shared.Core.Features.ExtendedAttributes.Commands.Validators;
using FluentPOS.Shared.Core.Interfaces.Serialization;
using Microsoft.Extensions.Localization;

namespace FluentPOS.Modules.Catalog.Core.Features.ExtendedAttributes.Validators
{
    public class AddProductExtendedAttributeCommandValidator : AddExtendedAttributeCommandValidator<Guid, Product>
    {
        public AddProductExtendedAttributeCommandValidator(IStringLocalizer<AddProductExtendedAttributeCommandValidator> localizer, IJsonSerializer jsonSerializer) : base(localizer, jsonSerializer)
        {
            // you can override the validation rules here
        }
    }
}