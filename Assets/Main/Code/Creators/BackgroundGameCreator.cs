using System;

public class BackgroundGameCreator
{
    private readonly ComputerPlayerCreator _computerPlayerCreator;

    public BackgroundGameCreator(ComputerPlayerCreator computerPlayerCreator)
    {
        _computerPlayerCreator = computerPlayerCreator ?? throw new ArgumentNullException(nameof(computerPlayerCreator));
    }

    public BackgroundGame Create()
    {
        return new BackgroundGame(_computerPlayerCreator.Create());
    }
}