using System.ComponentModel;

namespace TaskManagement.Domain.Util.Enumerators;
public enum TaskStatus
{
    Backlog,
    Doing,
    Done
}

public enum MessageType
{
    [Description("Create")]
    Create,
    [Description("Update")]
    Update,
    [Description("Delete")]
    Delete
}
