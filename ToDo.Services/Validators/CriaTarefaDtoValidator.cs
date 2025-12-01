using FluentValidation;
using ToDo.Services.DTOs;

namespace ToDo.Services.Validators
{
    public class CriaTarefaDtoValidator : AbstractValidator<CriarTarefaDTO>
    {
        public CriaTarefaDtoValidator()
        {
            RuleFor(x => x.Titulo)
                .NotEmpty().WithMessage("Título é obrigatório")
                .MinimumLength(5).WithMessage("Título deve ter ao menos 5 caracteres")
                .MaximumLength(200).WithMessage("Título não pode exceder 200 caracteres");

            RuleFor(x => x.Descricao)
                .MaximumLength(500).WithMessage("Descrição não pode exceder 500 caracteres");
        }
    }
}