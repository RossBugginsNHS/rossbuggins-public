
using System.Diagnostics;

[DebuggerDisplay("{DebuggerDisplay,nq}")]
public record TestEvent(string EntityId, string ImportantData)
{
    public string DebuggerDisplay => "DbgDisp " + EntityId + " " + ImportantData;
}

public record FirstNameSetEvent(string FirstName);