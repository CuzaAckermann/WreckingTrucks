using System;
using UnityEngine;

public class CartrigeBoxSpaceCreator
{
    private readonly CartrigeBoxFieldCreator _cartrigeBoxFieldCreator;
    private readonly MoverCreator _moverCreator;
    private readonly FillerCreator _fillerCreator;
    private readonly ModelFinalizerCreator _modelFinalizerCreator;

    public CartrigeBoxSpaceCreator(CartrigeBoxFieldCreator cartrigeBoxFieldCreator,
                                   MoverCreator moverCreator,
                                   FillerCreator fillerCreator,
                                   ModelFinalizerCreator modelFinalizerCreator)
    {
        _cartrigeBoxFieldCreator = cartrigeBoxFieldCreator ?? throw new ArgumentNullException(nameof(cartrigeBoxFieldCreator));
        _moverCreator = moverCreator ?? throw new ArgumentNullException(nameof(moverCreator));
        _fillerCreator = fillerCreator ?? throw new ArgumentNullException(nameof(fillerCreator));
        _modelFinalizerCreator = modelFinalizerCreator ?? throw new ArgumentNullException(nameof(modelFinalizerCreator));
    }

    public CartrigeBoxSpace Create(Transform fieldTransform, CartrigeBoxSpaceSettings cartrigeBoxSpaceSettings)
    {
        CartrigeBoxField field = _cartrigeBoxFieldCreator.Create(fieldTransform, cartrigeBoxSpaceSettings.FieldSettings.FieldSize);
        
        return new CartrigeBoxSpace(field,
                                    _moverCreator.Create(field, cartrigeBoxSpaceSettings.MoverSettings),
                                    _fillerCreator.Create(field),
                                    _modelFinalizerCreator.Create());
    }
}