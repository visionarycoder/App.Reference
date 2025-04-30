using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Praetor.Rules.Abstractions;
using RulesEngine.Models;

namespace Praetor.Rules;

public sealed class RulesEngineFactory(IRuleStore store, IMemoryCache cache)
{

  const string CacheKey = "CompiledRules";

  public async Task<RulesEngine.RulesEngine> GetAsync(CancellationToken ct = default)
  {
    if(cache.TryGetValue<RulesEngine.RulesEngine>(CacheKey, out var engine))
      return engine!;

    var defs = await store.GetAllAsync(ct);

    var workflows = defs
      .GroupBy(d => d.Name.Split(':')[0])
      .Select(g => new Workflow
      {
        WorkflowName = g.Key,
        Rules = g.Select(d => JsonSerializer.Deserialize<Rule>(d.JsonBody)!).ToList()
      }).ToArray();

    engine = new RulesEngine.RulesEngine(workflows, null);
    cache.Set(CacheKey, engine, TimeSpan.FromMinutes(10));   // 10-min cache

    return engine;
  }
}