namespace Novahome.Application.Condominiums.Update;

[UsedImplicitly]
public class CondominiumsUpdateValidator : AbstractValidator<CondominiumsUpdateRequest>
{
  public CondominiumsUpdateValidator()
  {
    RuleFor(x => x.Name)
      .NotEmpty()
      .MaximumLength(250);

    RuleFor(x => x.FiscalCode)
      .NotEmpty()
      .MaximumLength(11);
  }
}
