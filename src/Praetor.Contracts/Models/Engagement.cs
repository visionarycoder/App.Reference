using Praetor.Enums;

namespace Praetor.Contracts;

public record Engagement(Guid Id, string ProspectName, IReadOnlyList<PartyRef> Prospects, IReadOnlyList<ConflictRef> Conflicts, EngagementState State);