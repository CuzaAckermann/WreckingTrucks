using System;

public class CartrigeBoxFillerCreator
{
    private readonly StopwatchCreator _stopwatchCreator;
    //private readonly FillingStrategiesCreator _fillingStrategiesCreator;
    private readonly CartrigeBoxFillingCardCreator _fillingCardCreator;
    private readonly ModelProductionCreator _modelProducitonCreator;

    public CartrigeBoxFillerCreator(StopwatchCreator stopwatchCreator,
                                    CartrigeBoxFillingCardCreator cartrigeBoxFillingCardCreator,
                                    ModelProductionCreator modelProductionCreator)
    {
        _stopwatchCreator = stopwatchCreator ?? throw new ArgumentNullException(nameof(stopwatchCreator));
        //_fillingStrategiesCreator = fillingStrategiesCreator ?? throw new ArgumentNullException(nameof(fillingStrategiesCreator));
        _fillingCardCreator = cartrigeBoxFillingCardCreator ?? throw new ArgumentNullException(nameof(cartrigeBoxFillingCardCreator));
        _modelProducitonCreator = modelProductionCreator ?? throw new ArgumentNullException(nameof(modelProductionCreator));
    }

    public void SetFieldSettings(CartrigeBoxFieldSettings fieldSettings)
    {
        _fillingCardCreator.SetCartrigeBoxFieldSettings(fieldSettings);
    }

    public CartrigeBoxFieldFiller Create(CartrigeBoxField field, CartrigeBoxSpaceSettings cartrigeBoxSpaceSettings)
    {
        //FillingStrategy fillingStrategy = _fillingStrategiesCreator.Create(cartrigeBoxSpaceSettings.FillerSettings);
        //fillingStrategy.PrepareFilling(field, _fillingCardCreator.Create(cartrigeBoxSpaceSettings.FieldSettings.FieldSize));

        return new CartrigeBoxFieldFiller(_stopwatchCreator.Create(),
                                          cartrigeBoxSpaceSettings.FieldSettings.Frequency,
                                          field,
                                          _modelProducitonCreator.CreateCartrigeBoxFactory(),
                                          cartrigeBoxSpaceSettings.FieldSettings.AmountCartrigeBoxes);
    }
}