using System;

public class CreatedDispencerSignal
{
    private readonly Dispencer _dispencer;

    public CreatedDispencerSignal(Dispencer dispencer)
    {
        _dispencer = dispencer ?? throw new ArgumentNullException(nameof(dispencer));
    }

    public Dispencer Dispencer => _dispencer;
}