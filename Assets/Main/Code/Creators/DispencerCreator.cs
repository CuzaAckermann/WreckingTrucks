using System;
using System.Collections.Generic;
using UnityEngine;

public class DispencerCreator
{
    public event Action<Dispencer> Created;

    public Dispencer Create(CartrigeBoxField cartrigeBoxField, int startAmountCartrigeBoxes)
    {
        Dispencer dispencer = new Dispencer(cartrigeBoxField, startAmountCartrigeBoxes);

        Created?.Invoke(dispencer);

        return dispencer;
    }
}