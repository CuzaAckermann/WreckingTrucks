using System;
using UnityEngine;

public class CartrigeBoxSpaceCreator
{
    private readonly CartrigeBoxFieldCreator _cartrigeBoxFieldCreator;
    private readonly MoverCreator _moverCreator;
    private readonly FillerCreator _fillerCreator;
    private readonly CartrigeBoxFillingCardCreator _cartrigeBoxFillingCardCreator;

    public CartrigeBoxSpaceCreator(CartrigeBoxFieldCreator cartrigeBoxFieldCreator,
                                   MoverCreator moverCreator,
                                   FillerCreator fillerCreator,
                                   CartrigeBoxFillingCardCreator cartrigeBoxFillingCardCreator)
    {
        _cartrigeBoxFieldCreator = cartrigeBoxFieldCreator ?? throw new ArgumentNullException(nameof(cartrigeBoxFieldCreator));
        _moverCreator = moverCreator ?? throw new ArgumentNullException(nameof(moverCreator));
        _fillerCreator = fillerCreator ?? throw new ArgumentNullException(nameof(fillerCreator));
        _cartrigeBoxFillingCardCreator = cartrigeBoxFillingCardCreator ?? throw new ArgumentNullException(nameof(cartrigeBoxFillingCardCreator));
    }

    public CartrigeBoxSpace Create(Transform fieldTransform, CartrigeBoxSpaceSettings cartrigeBoxSpaceSettings)
    {
        CartrigeBoxField field = _cartrigeBoxFieldCreator.Create(fieldTransform,
                                                                 cartrigeBoxSpaceSettings.FieldSettings.FieldSize,
                                                                 cartrigeBoxSpaceSettings.FieldIntervals);
        
        return new CartrigeBoxSpace(field,
                                    _moverCreator.Create(field, cartrigeBoxSpaceSettings.MoverSettings),
                                    _fillerCreator.Create(cartrigeBoxSpaceSettings.FillerSettings,
                                                          field,
                                                          _cartrigeBoxFillingCardCreator.Create(cartrigeBoxSpaceSettings.FieldSettings)));
    }
}