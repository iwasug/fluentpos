﻿using System;
using FluentPOS.Modules.People.Core.Entities;
using FluentPOS.Shared.Core.Features.ExtendedAttributes.Commands.Validators;
using FluentPOS.Shared.Core.Interfaces.Serialization;
using Microsoft.Extensions.Localization;

namespace FluentPOS.Modules.People.Core.Features.ExtendedAttributes.Validators
{
    public class UpdateCustomerExtendedAttributeCommandValidator : UpdateExtendedAttributeCommandValidator<Guid, Customer>
    {
        public UpdateCustomerExtendedAttributeCommandValidator(IStringLocalizer<UpdateCustomerExtendedAttributeCommandValidator> localizer, IJsonSerializer jsonSerializer) : base(localizer, jsonSerializer)
        {
            // you can override the validation rules here
        }
    }
}