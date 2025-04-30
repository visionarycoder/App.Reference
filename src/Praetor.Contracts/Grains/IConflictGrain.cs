namespace Praetor.Contracts.Grains;

public interface IConflictGrain : IGrainWithGuidKey
{
  Task<ConflictDetails> GetAsync();
  Task LinkWaiverAsync(Guid waiverId);
}