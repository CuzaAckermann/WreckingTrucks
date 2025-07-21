using System;
using System.Collections.Generic;

public class Supplier : IModelPositionObserver
{
    public event Action<Model> PositionChanged;
    public event Action<List<Model>> PositionsChanged;

    public void AddCartrigeBox(CartrigeBox cartrigeBox)
    {
        PositionChanged?.Invoke(cartrigeBox);
    }
}