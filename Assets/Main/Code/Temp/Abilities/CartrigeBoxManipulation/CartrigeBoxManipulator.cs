using System;

public class CartrigeBoxManipulator
{
    private readonly CartrigeBoxManipulatorSettings _settings;

    private readonly Stopwatch _stopwatch;

    private readonly DispencerCreator _dispencerCreator;
    private readonly CartrigeBoxFillerCreator _fillerCreator;

    private StopwatchWaitingState _waitingState;

    private Dispencer _dispencer;
    private CartrigeBoxFieldFiller _fieldFiller;

    public CartrigeBoxManipulator(CartrigeBoxManipulatorSettings settings,
                                  Stopwatch stopwatchForTaking,
                                  DispencerCreator dispencerCreator,
                                  CartrigeBoxFillerCreator fillerCreator)
    {
        _settings = settings ? settings : throw new ArgumentNullException(nameof(settings));

        _stopwatch = stopwatchForTaking ?? throw new ArgumentNullException(nameof(stopwatchForTaking));

        _dispencerCreator = dispencerCreator ?? throw new ArgumentNullException(nameof(dispencerCreator));
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
            if (_dispencer.TryGetCartrigeBox(out CartrigeBox cartrigeBox))
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

        _dispencer.AddAmountAddedCartrigeBoxes(_settings.AmountForAdd);

        StartWaitingTakeCartrigeBoxes();
    }

    private void SubscribeToCreators()
    {
        _dispencerCreator.Created += SetDispercer;
        _fillerCreator.Created += SetFiller;
    }

    private void UnsubscribeFromCreators()
    {
        _dispencerCreator.Created -= SetDispercer;
        _fillerCreator.Created -= SetFiller;
    }

    private void SetDispercer(Dispencer dispencer)
    {
        _dispencer = dispencer;
    }

    private void SetFiller(CartrigeBoxFieldFiller filler)
    {
        _fieldFiller = filler;
    }
}