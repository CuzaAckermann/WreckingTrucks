using System;

public interface ICommandCreator
{
    public event Action<Command> CommandCreated;
}