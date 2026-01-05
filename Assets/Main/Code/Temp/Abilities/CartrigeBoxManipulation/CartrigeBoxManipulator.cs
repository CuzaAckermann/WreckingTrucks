using System;

public class CartrigeBoxManipulator
{
    private readonly CartrigeBoxManipulatorSettings _settings;

    private readonly Stopwatch _stopwatch;

    private readonly CartrigeBoxFieldCreator _fieldCreator;
    private readonly CartrigeBoxFillerCreator _fillerCreator;

    private StopwatchWaitingState _waitingState;

    private CartrigeBoxField _field;
    private CartrigeBoxFieldFiller _fieldFiller;

    public CartrigeBoxManipulator(CartrigeBoxManipulatorSettings settings,
                                  Stopwatch stopwatchForTaking,
                                  CartrigeBoxFieldCreator fieldCreator,
                                  CartrigeBoxFillerCreator fillerCreator)
    {
        _settings = settings ? settings : throw new ArgumentNullException(nameof(settings));

        _stopwatch = stopwatchForTaking ?? throw new ArgumentNullException(nameof(stopwatchForTaking));

        _fieldCreator = fieldCreator ?? throw new ArgumentNullException(nameof(fieldCreator));
        _fillerCreator = fillerCreator ?? throw new ArgumentNullException(nameof(fillerCreator));

        SubscribeToCreators();
    }

    public void Start()
    {
        StartWaitingTakeCartrigeBoxes();
    }

    public void Stop()
    {
        _waitingState.Exit();

        //UnsubscribeFromCreators();
    }

    private void StartWaitingTakeCartrigeBoxes()
    {
        _waitingState = new StopwatchWaitingState(_stopwatch, _settings.TimeForTaking);
        _waitingState.Enter(TakeCartrigeBoxes);
    }

    private void TakeCartrigeBoxes()
    {
        _waitingState.Exit();

        for (int i = 0; i < _settings.AmountForTaking; i++)
        {
            if (_field.TryGetCartrigeBox(out CartrigeBox cartrigeBox))
            {
                cartrigeBox.Destroy();
            }
            else
            {
                Logger.Log("CartrigeBoxField is empty");
                break;
            }
        }

        StartWaitingAddCartrigeBoxes();
    }

    private void StartWaitingAddCartrigeBoxes()
    {
        _waitingState = new StopwatchWaitingState(_stopwatch, _settings.TimeForAdd);

        _waitingState.Enter(AddCartrigeBoxes);
    }

    private void AddCartrigeBoxes()
    {
        _waitingState.Exit();

        _fieldFiller.AddAmountAddedCartrigeBoxes(_settings.AmountForAdd);

        StartWaitingTakeCartrigeBoxes();
    }

    private void SubscribeToCreators()
    {
        _fieldCreator.Created += SetField;
        _fillerCreator.Created += SetFiller;
    }

    private void UnsubscribeFromCreators()
    {
        _fieldCreator.Created -= SetField;
        _fillerCreator.Created -= SetFiller;
    }

    private void SetField(CartrigeBoxField field)
    {
        _field = field;
    }

    private void SetFiller(CartrigeBoxFieldFiller filler)
    {
        _fieldFiller = filler;
    }
}