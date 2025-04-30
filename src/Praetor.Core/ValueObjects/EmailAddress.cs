using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Praetor.ValueObjects;

public readonly record struct EmailAddress(string Value)
{
  
  public static EmailAddress Parse(string value) => value.Contains('@') 
    ? new(value) 
    : throw new ArgumentException("Invalid email", nameof(value));
  
  public override string ToString() => Value;

}