namespace Praetor.Rules;

public sealed class RulesGrain(RulesEngineFactory factory, ILogger<RulesGrain> log)
  : Grain, IRulesGrain
{
  public async Task<IEnumerable<Guid>> MatchConflictsAsync(
    Guid engagementId, IEnumerable<Guid> prospectPartyIds)
  {
    var engine = await factory.GetAsync();

    // Pass prospect IDs as fact
    var facts = new[]
    {
      new RulesEngine.Models.RuleParameter("Prospects",
        prospectPartyIds.ToHashSet())
    };

    // Single workflow = "ConflictCheck"
    var results = await engine.ExecuteAllRulesAsync("ConflictCheck", facts);

    // Each success result’s output payload is new ConflictId
    var ids = results
      .Where(r => r.IsSuccess && Guid.TryParse(r.Output?.ToString(), out _))
      .Select(r => Guid.Parse(r.Output!.ToString()!))
      .ToList();

    log.LogInformation("Conflict search for Engagement {EngId} yielded {Count} hit(s)",
      engagementId, ids.Count);

    return ids;
  }
}