﻿using FluentPOS.Modules.Identity.Core.Entities;
using FluentPOS.Shared.Core.Features.ExtendedAttributes.Commands.Validators;
using FluentPOS.Shared.Core.Interfaces.Serialization;
using Microsoft.Extensions.Localization;

namespace FluentPOS.Modules.Identity.Core.Features.ExtendedAttributes.Validators
{
    public class AddRoleExtendedAttributeCommandValidator : AddExtendedAttributeCommandValidator<string, FluentRole>
    {
        public AddRoleExtendedAttributeCommandValidator(IStringLocalizer<AddRoleExtendedAttributeCommandValidator> localizer, IJsonSerializer jsonSerializer) : base(localizer, jsonSerializer)
        {
            // you can override the validation rules here
        }
    }
}