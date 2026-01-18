using System;

public class JellyShackedSignal
{
    private readonly Jelly _jelly;

    public JellyShackedSignal(Jelly jelly)
    {
        _jelly = jelly ? jelly : throw new ArgumentNullException(nameof(jelly));
    }

    public Jelly Jelly => _jelly;
}