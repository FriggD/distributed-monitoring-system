using FluentValidation;
using MonitorService.Application.DTOs;

namespace MonitorService.Application.Validators;

public class CreateMetricValidator : AbstractValidator<CreateMetricDto>
{
    public CreateMetricValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Nome da métrica é obrigatório")
            .MaximumLength(100).WithMessage("Nome não pode ter mais de 100 caracteres");

        RuleFor(x => x.Value)
            .GreaterThanOrEqualTo(0).WithMessage("Valor deve ser maior ou igual a zero");

        RuleFor(x => x.Unit)
            .NotEmpty().WithMessage("Unidade é obrigatória")
            .MaximumLength(50).WithMessage("Unidade não pode ter mais de 50 caracteres");

        RuleFor(x => x.Source)
            .NotEmpty().WithMessage("Origem é obrigatória")
            .MaximumLength(200).WithMessage("Origem não pode ter mais de 200 caracteres");
    }
}
