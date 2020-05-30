using FluentValidation;
using ProcessamentoAcoes.Models;

namespace ProcessamentoAcoes.Validators
{
    public class TransacoesValidator : AbstractValidator <Transacao>
    {
        public TransacoesValidator()
        {
            RuleFor(c => c.Codigo).NotEmpty().WithMessage("Preencha o campo 'Codigo'");

            RuleFor(c => c.Valor).NotEmpty().WithMessage("Preencha o campo 'Valor'")
                .GreaterThan(0).WithMessage("O campo 'Valor' deve ser maior do 0");
        }
    }
}