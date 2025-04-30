using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Praetor.Enums;

namespace Praetor.Grains.State;

[GenerateSerializer]
public sealed record EngagementState(
  string ProspectName,
  HashSet<Guid> ProspectPartyIds,
  HashSet<Guid> ConflictIds,
  Enums.EngagementState WorkflowState,
  DateTimeOffset CreatedUtc);