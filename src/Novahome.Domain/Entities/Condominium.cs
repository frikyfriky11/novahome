namespace Novahome.Domain.Entities;

public sealed class Condominium : BaseAuditableEntity
{
    public Guid Id { get; set; }

    public string Name { get; set; } = default!;

    public string FiscalCode { get; set; } = default!;

    // TODO future navigation properties
    // public ICollection<CondominiumAddress> Addresses { get; set; } = default!;
    //
    // public ICollection<Person> Persons { get; set; } = default!;
    //
    // public ICollection<AdministrationPeriod> AdministrationPeriods { get; set; } = default!;
    //
    // public ICollection<FiscalYear> FiscalYears { get; set; } = default!;
    //
    // public ICollection<Supplier> Suppliers { get; set; } = default!;
    //
    // public ICollection<SupplierCategory> SupplierCategories { get; set; } = default!;
    //
    // public ICollection<Incident> Incidents { get; set; } = default!;
}
