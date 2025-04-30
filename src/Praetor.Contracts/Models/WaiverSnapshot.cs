using Praetor.Enums;

namespace Praetor.Contracts;

public record WaiverSnapshot(Guid Id, Guid ConflictId, WaiverState State, DateTimeOffset RequestedUtc, DateTimeOffset? RespondedUtc);