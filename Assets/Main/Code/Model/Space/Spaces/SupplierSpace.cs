using System;

public class SupplierSpace
{
    private readonly Supplier _supplier;
    private readonly Mover _mover;

    public SupplierSpace(Supplier supplier, Mover mover)
    {
        _supplier = supplier ?? throw new ArgumentNullException(nameof(supplier));
        _mover = mover ?? throw new ArgumentNullException(nameof(mover));
    }

    public void Clear()
    {
        _mover.Clear();
    }

    public void Enable()
    {
        _mover.Enable();
    }

    public void Disable()
    {
        _mover.Disable();
    }

    public void AddCartrigeBox(CartrigeBox cartrigeBox)
    {
        _supplier.AddCartrigeBox(cartrigeBox);
    }
}