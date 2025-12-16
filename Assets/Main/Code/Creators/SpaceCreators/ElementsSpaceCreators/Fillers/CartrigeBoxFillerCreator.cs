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

    public event Action<CartrigeBoxFieldFiller> Created;

    public CartrigeBoxFieldFiller Create(CartrigeBoxField field, int amountCartrigeBoxes)
    {
        //FillingStrategy fillingStrategy = _fillingStrategiesCreator.Create(cartrigeBoxSpaceSettings.FillerSettings);
        //fillingStrategy.PrepareFilling(field, _fillingCardCreator.Create(cartrigeBoxSpaceSettings.FieldSettings.FieldSize));

        CartrigeBoxFieldFiller cartrigeBoxFieldFiller = new CartrigeBoxFieldFiller(_stopwatchCreator.Create(),
                                                                                   0.01f,
                                                                                   field,
                                                                                   _modelProducitonCreator.CreateCartrigeBoxFactory(),
                                                                                   amountCartrigeBoxes);

        Created?.Invoke(cartrigeBoxFieldFiller);

        return cartrigeBoxFieldFiller;
    }
}