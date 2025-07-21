using System;

public class SupplierSpaceCreator
{
    private readonly SupplierCreator _supplierCreator;
    private readonly MoverCreator _moverCreator;

    public SupplierSpaceCreator(SupplierCreator supplierCreator, MoverCreator moverCreator)
    {
        _supplierCreator = supplierCreator ?? throw new ArgumentNullException(nameof(supplierCreator));
        _moverCreator = moverCreator ?? throw new ArgumentNullException(nameof(moverCreator));
    }

    public SupplierSpace Create(SupplierSpaceSettings supplierSpaceSettings)
    {
        Supplier supplier = _supplierCreator.Create();

        return new SupplierSpace(supplier, _moverCreator.Create(supplier, supplierSpaceSettings.MoverSettings));
    }
}