using Praetor.Enums;

namespace Praetor.Grains.State;

[GenerateSerializer]
public sealed record ConflictState(
  Guid PartyId,
  Guid EngagementId,
  string Reason,
  WaiverState WaiverStatus);