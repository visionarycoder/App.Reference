using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Praetor.Rules.Abstractions;

public interface IRuleStore
{
  Task<IReadOnlyList<Model.RuleDefinition>> GetAllAsync(CancellationToken ct = default);
  Task<Model.RuleDefinition?> GetAsync(string name, CancellationToken ct = default);
}