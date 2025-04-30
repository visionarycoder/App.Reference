namespace Praetor.Contracts.Grains;

public interface IEngagementGrain : IGrainWithGuidKey
{
  Task AddProspectPartiesAsync(IEnumerable<PartyInput> parties);
  Task StartSearchAsync();
  Task<Engagement> GetAsync();
}