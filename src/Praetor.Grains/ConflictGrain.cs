using Praetor.Contracts;
using Praetor.Contracts.Grains;
using Praetor.Grains.State;

namespace Praetor.Grains;

public sealed class ConflictGrain 
  : Grain<ConflictState>, IConflictGrain
{
  public Task<ConflictDetails> GetAsync() =>
    Task.FromResult(new ConflictDetails(Id: this.GetPrimaryKey(), PartyId: State.PartyId, EngagementId: State.EngagementId, Reason: State.Reason, WaiverStatus: State.WaiverStatus));

  public Task LinkWaiverAsync(Guid waiverId)
  {
    // Just update status flag; WaiverGrain owns real state
    return Task.CompletedTask;
  }
}