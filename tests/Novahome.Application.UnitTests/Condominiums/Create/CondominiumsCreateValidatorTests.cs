using Novahome.Application.Condominiums.Create;

namespace Novahome.Application.UnitTests.Condominiums.Create;

public class CondominiumsCreateValidatorTests
{
  private CondominiumsCreateValidator _sut = null!;

  [SetUp]
  public void SetUp()
  {
    _sut = new CondominiumsCreateValidator();
  }

  [Test]
  public void Validator_ShouldReturnError_WhenNameIsEmpty()
  {
    CondominiumsCreateRequest request = new(string.Empty, "fiscalCode");
    TestValidationResult<CondominiumsCreateRequest>? result = _sut.TestValidate(request);
    result.ShouldHaveValidationErrorFor(v => v.Name);
  }

  [Test]
  public void Validator_ShouldReturnError_WhenNameIsTooLong()
  {
    CondominiumsCreateRequest request = new(new string('a', 251), "fiscalCode");
    TestValidationResult<CondominiumsCreateRequest>? result = _sut.TestValidate(request);
    result.ShouldHaveValidationErrorFor(v => v.Name);
  }

  [Test]
  public void Validator_ShouldReturnError_WhenFiscalCodeIsEmpty()
  {
    CondominiumsCreateRequest request = new("name", string.Empty);
    TestValidationResult<CondominiumsCreateRequest>? result = _sut.TestValidate(request);
    result.ShouldHaveValidationErrorFor(v => v.FiscalCode);
  }

  [Test]
  public void Validator_ShouldReturnError_WhenFiscalCodeIsTooLong()
  {
    CondominiumsCreateRequest request = new("name", new string('a', 12));
    TestValidationResult<CondominiumsCreateRequest>? result = _sut.TestValidate(request);
    result.ShouldHaveValidationErrorFor(v => v.FiscalCode);
  }
}
