﻿using FluentValidation;

namespace PooPosting.Api.Models.Dtos.Picture.Validators;

public class UpdatePictureTagsDtoValidator: AbstractValidator<UpdatePictureTagsDto>
{
    public UpdatePictureTagsDtoValidator()
    {
        RuleFor(t => t.Tags)
            .Must(tags =>
                !(tags.Any(t => t.Length > 25)) &&
                !(tags.Length > 4))
            .WithMessage("Maximum tag count is 4, maximum tag length is 25.");
    }
}