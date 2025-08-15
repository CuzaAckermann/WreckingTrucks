using System;

public class SupplierSpaceCreator
{
    private readonly SupplierCreator _supplierCreator;
    private readonly MoverCreator _moverCreator;
    private readonly RotatorCreator _rotatorCreator;

    public SupplierSpaceCreator(SupplierCreator supplierCreator,
                                MoverCreator moverCreator,
                                RotatorCreator rotatorCreator)
    {
        _supplierCreator = supplierCreator ?? throw new ArgumentNullException(nameof(supplierCreator));
        _moverCreator = moverCreator ?? throw new ArgumentNullException(nameof(moverCreator));
        _rotatorCreator = rotatorCreator ?? throw new ArgumentNullException(nameof(rotatorCreator));
    }

    public SupplierSpace Create(SupplierSpaceSettings supplierSpaceSettings)
    {
        Supplier supplier = _supplierCreator.Create();

        return new SupplierSpace(supplier,
                                 _moverCreator.Create(supplier, supplierSpaceSettings.MoverSettings),
                                 _rotatorCreator.Create(supplier, supplierSpaceSettings.RotatorSettings));
    }
}