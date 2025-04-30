using Praetor.Grains.State;

namespace Praetor.Grains;

public sealed class RelationshipGrain : Grain<RelationshipState>, IRelationshipGrain
{
  public override Task OnActivateAsync(CancellationToken _) =>
    Task.CompletedTask;

  public async Task RegisterAsync(RelationshipRecord rel)
  {
    State = new RelationshipState(rel);
    await WriteStateAsync();

    // Link back into Party
    var pg = GrainFactory.GetGrain<IPartyGrain>(rel.PartyId);
    var party = await pg.GetAsync();
    var updated = party.Relationships.Append(rel).ToHashSet();
    await pg.RegisterAsync(new PartyInput(party.Name, party.Kind, party.Aliases));
  }
}