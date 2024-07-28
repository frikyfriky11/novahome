namespace Novahome.Domain.Entities;

public sealed class User
{
  public Guid Id { get; set; }

  public string EmailAddress { get; set; } = default!;

  public string GivenName { get; set; } = default!;

  public string FamilyName { get; set; } = default!;

  // TODO future navigation properties
  // public ICollection<Administrator> Administrators { get; set; } = default!;
  //
  // public ICollection<Person> Persons { get; set; } = default!;

  public ICollection<Condominium> CreatedCondominiums { get; set; } = default!;

  public ICollection<Condominium> LastModifiedCondominiums { get; set; } = default!;
}
