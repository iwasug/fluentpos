﻿using FluentPOS.Shared.Core.Contracts;
using FluentPOS.Shared.Core.Extensions;
using FluentPOS.Shared.Core.Interfaces.Serialization;
using FluentPOS.Shared.DTOs.ExtendedAttributes;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace FluentPOS.Shared.Core.Features.ExtendedAttributes.Commands.Validators
{
    public class AddExtendedAttributeCommandValidator<TEntityId, TEntity> : AbstractValidator<AddExtendedAttributeCommand<TEntityId, TEntity>>
        where TEntity : class, IEntity<TEntityId>
    {
        public AddExtendedAttributeCommandValidator(IStringLocalizer localizer, IJsonSerializer jsonSerializer)
        {
            RuleFor(request => request.EntityId)
                .NotEqual(default(TEntityId)).WithMessage(x => localizer["The {PropertyName} property cannot be default."]);
            RuleFor(request => request.Key)
                .NotEmpty().WithMessage(x => localizer["The {PropertyName} property cannot be empty."]);

            When(request => request.Type == ExtendedAttributeType.Decimal, () =>
            {
                RuleFor(request => request.Decimal).NotNull().WithMessage(x => string.Format(localizer["Decimal value is required using {0} type!"], x.Type.ToString()));
                RuleFor(request => request.Text).Null().WithMessage(x => string.Format(localizer["Text value should be null using {0} type!"], x.Type.ToString()));
                RuleFor(request => request.DateTime).Null().WithMessage(x => string.Format(localizer["DateTime value should be null using {0} type!"], x.Type.ToString()));
                RuleFor(request => request.Json).Null().WithMessage(x => string.Format(localizer["Json value should be null using {0} type!"], x.Type.ToString()));
                RuleFor(request => request.Boolean).Null().WithMessage(x => string.Format(localizer["Boolean value should be null using {0} type!"], x.Type.ToString()));
                RuleFor(request => request.Integer).Null().WithMessage(x => string.Format(localizer["Integer value should be null using {0} type!"], x.Type.ToString()));
            });
            When(request => request.Type == ExtendedAttributeType.Text, () =>
            {
                RuleFor(request => request.Text).NotNull().WithMessage(x => string.Format(localizer["Text value is required using {0} type!"], x.Type.ToString()));
                RuleFor(request => request.Decimal).Null().WithMessage(x => string.Format(localizer["Decimal value should be null using {0} type!"], x.Type.ToString()));
                RuleFor(request => request.DateTime).Null().WithMessage(x => string.Format(localizer["DateTime value should be null using {0} type!"], x.Type.ToString()));
                RuleFor(request => request.Json).Null().WithMessage(x => string.Format(localizer["Json value should be null using {0} type!"], x.Type.ToString()));
                RuleFor(request => request.Boolean).Null().WithMessage(x => string.Format(localizer["Boolean value should be null using {0} type!"], x.Type.ToString()));
                RuleFor(request => request.Integer).Null().WithMessage(x => string.Format(localizer["Integer value should be null using {0} type!"], x.Type.ToString()));
            });
            When(request => request.Type == ExtendedAttributeType.DateTime, () =>
            {
                RuleFor(request => request.DateTime).NotNull().WithMessage(x => string.Format(localizer["DateTime value is required using {0} type!"], x.Type.ToString()));
                RuleFor(request => request.Decimal).Null().WithMessage(x => string.Format(localizer["Decimal value should be null using {0} type!"], x.Type.ToString()));
                RuleFor(request => request.Text).Null().WithMessage(x => string.Format(localizer["Text value should be null using {0} type!"], x.Type.ToString()));
                RuleFor(request => request.Json).Null().WithMessage(x => string.Format(localizer["Json value should be null using {0} type!"], x.Type.ToString()));
                RuleFor(request => request.Boolean).Null().WithMessage(x => string.Format(localizer["Boolean value should be null using {0} type!"], x.Type.ToString()));
                RuleFor(request => request.Integer).Null().WithMessage(x => string.Format(localizer["Integer value should be null using {0} type!"], x.Type.ToString()));
            });
            When(request => request.Type == ExtendedAttributeType.Json, () =>
            {
                RuleFor(request => request.Json).MustBeJson(jsonSerializer)
                    .WithMessage(x => string.Format(localizer["Json value must be a valid JSON string using {0} type!"], x.Type.ToString()));
                RuleFor(request => request.Json).NotNull().WithMessage(x => string.Format(localizer["Json value is required using {0} type!"], x.Type.ToString()));
                RuleFor(request => request.Decimal).Null().WithMessage(x => string.Format(localizer["Decimal value should be null using {0} type!"], x.Type.ToString()));
                RuleFor(request => request.Text).Null().WithMessage(x => string.Format(localizer["Text value should be null using {0} type!"], x.Type.ToString()));
                RuleFor(request => request.DateTime).Null().WithMessage(x => string.Format(localizer["DateTime value should be null using {0} type!"], x.Type.ToString()));
                RuleFor(request => request.Boolean).Null().WithMessage(x => string.Format(localizer["Boolean value should be null using {0} type!"], x.Type.ToString()));
                RuleFor(request => request.Integer).Null().WithMessage(x => string.Format(localizer["Integer value should be null using {0} type!"], x.Type.ToString()));
            });
            When(request => request.Type == ExtendedAttributeType.Boolean, () =>
            {
                RuleFor(request => request.Boolean).NotNull().WithMessage(x => string.Format(localizer["Boolean value is required using {0} type!"], x.Type.ToString()));
                RuleFor(request => request.Decimal).Null().WithMessage(x => string.Format(localizer["Decimal value should be null using {0} type!"], x.Type.ToString()));
                RuleFor(request => request.Text).Null().WithMessage(x => string.Format(localizer["Text value should be null using {0} type!"], x.Type.ToString()));
                RuleFor(request => request.DateTime).Null().WithMessage(x => string.Format(localizer["DateTime value should be null using {0} type!"], x.Type.ToString()));
                RuleFor(request => request.Json).Null().WithMessage(x => string.Format(localizer["Json value should be null using {0} type!"], x.Type.ToString()));
                RuleFor(request => request.Integer).Null().WithMessage(x => string.Format(localizer["Integer value should be null using {0} type!"], x.Type.ToString()));
            });
            When(request => request.Type == ExtendedAttributeType.Integer, () =>
            {
                RuleFor(request => request.Integer).NotNull().WithMessage(x => string.Format(localizer["Integer value is required using {0} type!"], x.Type.ToString()));
                RuleFor(request => request.Decimal).Null().WithMessage(x => string.Format(localizer["Decimal value should be null using {0} type!"], x.Type.ToString()));
                RuleFor(request => request.Text).Null().WithMessage(x => string.Format(localizer["Text value should be null using {0} type!"], x.Type.ToString()));
                RuleFor(request => request.DateTime).Null().WithMessage(x => string.Format(localizer["DateTime value should be null using {0} type!"], x.Type.ToString()));
                RuleFor(request => request.Json).Null().WithMessage(x => string.Format(localizer["Json value should be null using {0} type!"], x.Type.ToString()));
                RuleFor(request => request.Boolean).Null().WithMessage(x => string.Format(localizer["Boolean value should be null using {0} type!"], x.Type.ToString()));
            });
        }
    }
}