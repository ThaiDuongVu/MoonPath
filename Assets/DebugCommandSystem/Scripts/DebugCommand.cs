using System;

// Base class for debug command
public class DebugCommandBase
{
    protected string ID { get; set; }
    protected string Description { get; set; }
    public string Format { get; protected set; }
}

// If the command does not require any parameters
public class DebugCommand : DebugCommandBase
{
    private readonly Action _action;

    public DebugCommand(string id, string description, string format, Action action)
    {
        ID = id;
        Description = description;
        Format = format;

        _action = action;
    }

    public void Invoke()
    {
        _action.Invoke();
    }
}

// If the command does require a parameter
public abstract class DebugCommand<T> : DebugCommandBase
{
    private readonly Action<T> _action;

    protected DebugCommand(string id, string description, string format, Action<T> action)
    {
        ID = id;
        Description = description;
        Format = format;

        _action = action;
    }

    public void Invoke(T value)
    {
        _action.Invoke(value);
    }
}