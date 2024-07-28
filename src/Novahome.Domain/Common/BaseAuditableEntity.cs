namespace Novahome.Domain.Common;

/// <summary>
///   This abstract class represents a base entity that can be inherited from other
///   classes to obtain default properties that enable the change tracker of EF Core
///   to track the creation and modification of entities at the Infrastructure layer.
/// </summary>
public abstract class BaseAuditableEntity
{
  /// <summary>
  ///   The date when the entity was first created
  /// </summary>
  public DateTimeOffset CreatedOn { get; set; }

  /// <summary>
  ///   The user that first created the entity.
  ///   It can be null if the entity was system-generated.
  /// </summary>
  public User? CreatedBy { get; set; } = null!;

  /// <summary>
  ///   The user id that first created the entity.
  ///   It can be null if the entity was system-generated.
  /// </summary>
  public Guid? CreatedById { get; set; }

  /// <summary>
  ///   The date when the entity was last modified.
  ///   It can be null if the entity has never been modified.
  /// </summary>
  public DateTimeOffset? LastModifiedOn { get; set; }

  /// <summary>
  ///   The user that last modified the entity.
  ///   It can be null if the entity has never been modified or if it was system-generated.
  /// </summary>
  public User? LastModifiedBy { get; set; } = null!;

  /// <summary>
  ///   The user id that last modified the entity.
  ///   It can be null if the entity has never been modified or if it was system-generated.
  /// </summary>
  public Guid? LastModifiedById { get; set; }
}
