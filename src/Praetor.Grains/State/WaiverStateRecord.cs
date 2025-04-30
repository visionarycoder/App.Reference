using Praetor.Enums;

namespace Praetor.Grains.State;

[GenerateSerializer]
public sealed record WaiverStateRecord(
  Guid ConflictId,
  WaiverState Status,
  DateTimeOffset RequestedUtc,
  DateTimeOffset? RespondedUtc,
  string ToEmail);