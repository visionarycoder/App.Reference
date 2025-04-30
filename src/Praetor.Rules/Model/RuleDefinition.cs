using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Praetor.Rules.Model;

public sealed record RuleDefinition(string Name, string JsonBody, DateTimeOffset UpdatedUtc)
{
  public string ToJson() => JsonBody;
  public JsonElement ToElement() => JsonSerializer.Deserialize<JsonElement>(JsonBody);
}
