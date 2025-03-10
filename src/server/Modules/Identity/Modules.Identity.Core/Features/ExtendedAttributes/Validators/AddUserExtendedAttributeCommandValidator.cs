﻿using FluentPOS.Modules.Identity.Core.Entities;
using FluentPOS.Shared.Core.Features.ExtendedAttributes.Commands.Validators;
using FluentPOS.Shared.Core.Interfaces.Serialization;
using Microsoft.Extensions.Localization;

namespace FluentPOS.Modules.Identity.Core.Features.ExtendedAttributes.Validators
{
    public class AddUserExtendedAttributeCommandValidator : AddExtendedAttributeCommandValidator<string, FluentUser>
    {
        public AddUserExtendedAttributeCommandValidator(IStringLocalizer<AddUserExtendedAttributeCommandValidator> localizer, IJsonSerializer jsonSerializer) : base(localizer, jsonSerializer)
        {
            // you can override the validation rules here
        }
    }
}