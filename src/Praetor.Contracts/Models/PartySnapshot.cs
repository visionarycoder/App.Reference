using Praetor.Enums;

namespace Praetor.Contracts;

public record PartySnapshot(Guid Id, string Name, PartyKind Kind, IReadOnlySet<string> Aliases, IReadOnlySet<Relationship> Relationships);