namespace Praetor.ValueObjects;

/// <summary>
/// Domain-specific wrapper around <see cref="Guid"/> so
///   • method signatures are self-documenting  
///   • you can’t mix it with other ids by accident
/// </summary>
public readonly record struct EngagementId(Guid Value)
{
  /// <summary>Create a brand-new ID.</summary>
  public static EngagementId New() => new(Guid.NewGuid());

  /// <summary>Parse from the canonical string form.</summary>
  public static EngagementId Parse(string text) => new(Guid.Parse(text));

  public static bool TryParse(string? text, out EngagementId id)
  {
    var ok = Guid.TryParse(text, out var g);
    id = ok ? new EngagementId(g) : default;
    return ok;
  }

  public override string ToString() => Value.ToString();

  // Implicit conversions so callers can still pass / receive Guid when needed
  public static implicit operator Guid(EngagementId id) => id.Value;
  public static implicit operator EngagementId(Guid g) => new(g);
}

public readonly record struct PartyAlias(string Value)
{
  public override string ToString() => Value;
  public static PartyAlias From(string text) => new(text.Trim().ToUpperInvariant());

}