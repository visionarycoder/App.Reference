using Praetor.Enums;

namespace Praetor.Contracts;

public record ConflictDetails(Guid Id, Guid PartyId, Guid EngagementId, string Reason, WaiverState WaiverStatus);