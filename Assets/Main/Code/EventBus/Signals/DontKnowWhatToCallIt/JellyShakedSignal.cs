using System;

public class JellyShakedSignal : EventBusSignal
{
    private readonly Jelly _jelly;

    public JellyShakedSignal(Jelly jelly)
    {
        _jelly = jelly ? jelly : throw new ArgumentNullException(nameof(jelly));
    }

    public Jelly Jelly => _jelly;
}