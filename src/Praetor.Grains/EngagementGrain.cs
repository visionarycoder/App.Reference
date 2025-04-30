using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using Praetor.Contracts;
using Praetor.Contracts.Grains;
using Praetor.Enums;
using Praetor.Grains.State;
using EngagementState = Praetor.Enums.EngagementState;

namespace Praetor.Grains;
public sealed class EngagementGrain(ILogger<EngagementGrain> log)
  : Grain<State.EngagementState>, IEngagementGrain
{
  public override async Task OnActivateAsync(CancellationToken ct)
  {
    if(State is null)
    {
      State = new State.EngagementState(string.Empty, [], [], EngagementState.Draft, DateTimeOffset.UtcNow);
      await WriteStateAsync();
    }
  }

  public async Task AddProspectPartiesAsync(IEnumerable<PartyInput> parties)
  {
    foreach(var p in parties)
    {
      var pid = Guid.NewGuid();
      var pg = GrainFactory.GetGrain<IPartyGrain>(pid);
      await pg.RegisterAsync(p);
      State.ProspectPartyIds.Add(pid);
    }
    await WriteStateAsync();
  }

  public async Task StartSearchAsync()
  {
    State = State with { WorkflowState = EngagementState.Searching };
    await WriteStateAsync();

    // Fan-out search to RulesGrain
    var rules = GrainFactory.GetGrain<IRulesGrain>(0);
    var hits = await rules.MatchConflictsAsync(this.GetPrimaryKey(), State.ProspectPartyIds);

    foreach(var hit in hits)
    {
      State.ConflictIds.Add(hit);
      var cg = GrainFactory.GetGrain<IConflictGrain>(hit);
      await cg.LinkWaiverAsync(Guid.Empty); // not requested yet
    }
    State = State with
    {
      WorkflowState = State.ConflictIds.Any()
            ? EngagementState.AwaitingWaivers
            : EngagementState.Cleared
    };
    await WriteStateAsync();
  }

  public Task<Engagement> GetAsync()
  {
    var dto = new Engagement(
        Id: this.GetPrimaryKey(),
        ProspectName: State.ProspectName,
        Prospects: State.ProspectPartyIds
                             .Select(pid => new PartyRef(pid, ""))
                             .ToList(),
        Conflicts: State.ConflictIds
                             .Select(cid => new ConflictRef(cid, "", ""))
                             .ToList(),
        State: State.WorkflowState);
    return Task.FromResult(dto);
  }
}