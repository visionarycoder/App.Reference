namespace Praetor.Contracts.Grains;

public interface IWaiverGrain : IGrainWithGuidKey
{
  Task RequestAsync(Guid conflictId, string toEmail);
  Task GrantAsync();
  Task RefuseAsync();
  Task<WaiverSnapshot> GetAsync();
}