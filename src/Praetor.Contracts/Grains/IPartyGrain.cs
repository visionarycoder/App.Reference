namespace Praetor.Contracts.Grains;

public interface IPartyGrain : IGrainWithGuidKey
{
  Task RegisterAsync(PartyInput input);
  Task<PartySnapshot> GetAsync();
}