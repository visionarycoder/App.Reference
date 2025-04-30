namespace Praetor.Contracts.Grains;

public interface IRelationshipGrain : IGrainWithStringKey
{
  // key = $"{employeeId}:{partyId}"
  Task RegisterAsync(Relationship rel);
}