using Novahome.Application.Condominiums.Update;

namespace Novahome.Application.UnitTests.Condominiums.Update;

public class CondominiumsUpdateValidatorTests
{
  private CondominiumsUpdateValidator _sut = null!;

  [SetUp]
  public void SetUp()
  {
    _sut = new CondominiumsUpdateValidator();
  }

  [Test]
  public void Validator_ShouldReturnError_WhenNameIsEmpty()
  {
    CondominiumsUpdateRequest request = new(string.Empty, "fiscalCode");
    TestValidationResult<CondominiumsUpdateRequest>? result = _sut.TestValidate(request);
    result.ShouldHaveValidationErrorFor(v => v.Name);
  }

  [Test]
  public void Validator_ShouldReturnError_WhenNameIsTooLong()
  {
    CondominiumsUpdateRequest request = new(new string('a', 251), "fiscalCode");
    TestValidationResult<CondominiumsUpdateRequest>? result = _sut.TestValidate(request);
    result.ShouldHaveValidationErrorFor(v => v.Name);
  }

  [Test]
  public void Validator_ShouldReturnError_WhenFiscalCodeIsEmpty()
  {
    CondominiumsUpdateRequest request = new("name", string.Empty);
    TestValidationResult<CondominiumsUpdateRequest>? result = _sut.TestValidate(request);
    result.ShouldHaveValidationErrorFor(v => v.FiscalCode);
  }

  [Test]
  public void Validator_ShouldReturnError_WhenFiscalCodeIsTooLong()
  {
    CondominiumsUpdateRequest request = new("name", new string('a', 12));
    TestValidationResult<CondominiumsUpdateRequest>? result = _sut.TestValidate(request);
    result.ShouldHaveValidationErrorFor(v => v.FiscalCode);
  }
}
