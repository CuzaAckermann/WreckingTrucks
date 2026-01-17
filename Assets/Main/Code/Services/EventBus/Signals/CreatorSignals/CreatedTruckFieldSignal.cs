using System;

public class CreatedTruckFieldSignal
{
    private readonly TruckField _truckField;

    public CreatedTruckFieldSignal(TruckField truckField)
    {
        _truckField = truckField ?? throw new ArgumentNullException(nameof(truckField));
    }

    public TruckField TruckField => _truckField;
}
