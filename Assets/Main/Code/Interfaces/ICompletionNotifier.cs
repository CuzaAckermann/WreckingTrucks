using System;

public interface ICompletionNotifier
{
    public event Action Completed;
}