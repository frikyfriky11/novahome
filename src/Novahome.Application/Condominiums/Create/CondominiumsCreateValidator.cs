namespace Novahome.Application.Condominiums.Create;

[UsedImplicitly]
public class CondominiumsCreateValidator : AbstractValidator<CondominiumsCreateRequest>
{
  public CondominiumsCreateValidator()
  {
    RuleFor(x => x.Name)
      .NotEmpty()
      .MaximumLength(250);

    RuleFor(x => x.FiscalCode)
      .NotEmpty()
      .MaximumLength(11);
  }
}
