using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Praetor.Domain;

public abstract record DomainEvent(DateTimeOffset UtcTime);

public interface IHasDomainEvents
{
  IReadOnlyCollection<DomainEvent> DomainEvents { get; }
  void ClearDomainEvents();
}