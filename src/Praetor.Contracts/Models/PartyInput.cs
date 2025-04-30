using Praetor.Enums;

namespace Praetor.Contracts;

public record PartyInput(string Name, PartyKind Kind, IEnumerable<string> Aliases);