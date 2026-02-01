using System;

public interface ICommandCreator : IDestroyable
{
    public event Action<Command> CommandCreated;
}