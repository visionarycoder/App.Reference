using Microsoft.Extensions.Logging;
using Praetor.Contracts;
using Praetor.Contracts.Grains;
using Praetor.Enums;
using Praetor.Grains.State;

namespace Praetor.Grains;

public sealed class PartyGrain(ILogger<PartyGrain> log)
  : Grain<PartyState>, IPartyGrain
{
  
  public override async Task OnActivateAsync(CancellationToken _) => State ??= new PartyState("", PartyKind.Person, [], []);

  public async Task RegisterAsync(PartyInput input)
  {
    State = new PartyState(input.Name, input.Kind, input.Aliases.ToHashSet(StringComparer.OrdinalIgnoreCase), State.Relationships);
    await WriteStateAsync();
  }

  public Task<PartySnapshot> GetAsync() =>
    Task.FromResult(new PartySnapshot(Id: this.GetPrimaryKey(), Name: State.Name, Kind: State.Kind, Aliases: State.Aliases, Relationships: State.Relationships));

}