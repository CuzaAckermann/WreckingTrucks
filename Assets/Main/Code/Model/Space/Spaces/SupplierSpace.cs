using System;
using System.Collections.Generic;

public class SupplierSpace : IModelDestroyNotifier
{
    private readonly Supplier _supplier;
    private readonly Mover _mover;
    private readonly Rotator _rotator;

    public SupplierSpace(Supplier supplier, Mover mover, Rotator rotator)
    {
        _supplier = supplier ?? throw new ArgumentNullException(nameof(supplier));
        _mover = mover ?? throw new ArgumentNullException(nameof(mover));
        _rotator = rotator ?? throw new ArgumentNullException(nameof(rotator));
    }

    public event Action<Model> ModelDestroyRequested;
    public event Action<IModel> InterfaceModelDestroyRequested;
    public event Action<IReadOnlyList<Model>> ModelsDestroyRequested;
    public event Action<IReadOnlyList<IModel>> InterfaceModelsDestroyRequested;

    public void Clear()
    {
        ModelsDestroyRequested?.Invoke(_supplier.CartrigeBoxes);

        _supplier.Clear();
        _mover.Clear();
        _rotator.Clear();
    }

    public void Enable()
    {
        _mover.Enable();
        _rotator.Enable();
    }

    public void Disable()
    {
        _mover.Disable();
        _rotator.Disable();
    }

    public void AddCartrigeBox(CartrigeBox cartrigeBox)
    {
        _supplier.AddCartrigeBox(cartrigeBox);
    }
}