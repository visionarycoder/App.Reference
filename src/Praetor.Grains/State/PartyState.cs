using Praetor.Enums;

namespace Praetor.Grains.State;

[GenerateSerializer]
public sealed record PartyState(
  string Name,
  PartyKind Kind,
  HashSet<string> Aliases,
  HashSet<Contracts.Relationship> Relationships);