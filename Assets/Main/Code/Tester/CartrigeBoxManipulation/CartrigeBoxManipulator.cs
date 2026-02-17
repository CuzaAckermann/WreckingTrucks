using System;
using UnityEngine;

public class CartrigeBoxManipulator : MonoBehaviourSubscriber, ICommandCreator
{
    [Header("Settings")]
    [SerializeField] private CartrigeBoxManipulatorSettings _settings;

    [Header("Buttons")]
    [SerializeField] private GameButton _addButton;
    [SerializeField] private GameButton _takeButton;
    [SerializeField] private GameButton _switchButton;

    private Dispencer _dispencer;

    private EventBus _eventBus;
    private IInput _developerInput;

    private Command _currentCommand;

    private bool _isActivated;

    private Subscriber _buttonsSubscriber;
    private SubscriberWithCondition _dispencerSubscriber;

    public void Init(EventBus eventBus, IInput developerInput)
    {
        Validator.ValidateNotNull(eventBus, developerInput);

        _eventBus = eventBus;
        _developerInput = developerInput;

        OffButtons();

        _isActivated = false;

        _buttonsSubscriber = new Subscriber(SubscribeToButtons, UnsubscribeFromButtons);
        _dispencerSubscriber = new SubscriberWithCondition(SubscribeToDispencer, UnsubscribeFromDispencer);

        Init();
    }

    public event Action<IDestroyable> Destroyed;

    public event Action<Command> CommandCreated;

    public void Destroy()
    {
        Destroyed?.Invoke(this);
    }

    protected override void Subscribe()
    {
        _buttonsSubscriber.Subscribe();

        _eventBus?.Subscribe<CreatedSignal<Dispencer>>(SetDispencer);

        _dispencerSubscriber.Subscribe();
    }

    protected override void Unsubscribe()
    {
        _buttonsSubscriber.Unsubscribe();

        _eventBus?.Unsubscribe<CreatedSignal<Dispencer>>(SetDispencer);

        _dispencerSubscriber.Unsubscribe();
    }

    private void SubscribeToButtons()
    {
        _developerInput.SwitchUiButton.Pressed += SwitchButtons;

        _addButton.Pressed += OnPressedAddButton;
        _takeButton.Pressed += OnPressedTakeButton;
        _switchButton.Pressed += Switch;
    }

    private void UnsubscribeFromButtons()
    {
        _developerInput.SwitchUiButton.Pressed -= SwitchButtons;

        _addButton.Pressed -= OnPressedAddButton;
        _takeButton.Pressed -= OnPressedTakeButton;
        _switchButton.Pressed -= Switch;
    }

    private void SwitchButtons()
    {
        _isActivated = _isActivated == false;

        _addButton.Switch(_isActivated);
        _takeButton.Switch(_isActivated);
        _switchButton.Switch(_isActivated);
    }

    private void OnPressedAddButton()
    {
        if (_dispencer == null)
        {
            return;
        }

        _dispencer.AddAmountAddedCartrigeBoxes(_settings.AmountForAdd);
    }

    private void OnPressedTakeButton()
    {
        if (_dispencer == null)
        {
            return;
        }

        for (int i = 0; i < _settings.AmountForTaking; i++)
        {
            if (_dispencer.TryGetCartrigeBox(out CartrigeBox cartrigeBox))
            {
                cartrigeBox.Destroy();
            }
        }
    }

    private void Switch()
    {
        if (_dispencer == null)
        {
            return;
        }

        _isActivated = _isActivated == false;

        if (_isActivated)
        {
            SendCommand(TakeCartrigeBoxes, _settings.TimeForTaking);
        }
        else
        {
            CancelCommand();
        }
    }

    private void TakeCartrigeBoxes()
    {
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

        SendCommand(AddCartrigeBoxes, _settings.TimeForAdd);
    }

    private void AddCartrigeBoxes()
    {
        _dispencer.AddAmountAddedCartrigeBoxes(_settings.AmountForAdd);

        SendCommand(TakeCartrigeBoxes, _settings.TimeForTaking);
    }

    private void SetDispencer(CreatedSignal<Dispencer> createdDispencerSignal)
    {
        _dispencer = createdDispencerSignal.Creatable;

        _dispencerSubscriber.Subscribe();
        OnButtons();
    }

    private bool SubscribeToDispencer()
    {
        if (_dispencer == null)
        {
            return false;
        }

        _dispencer.Cleared += OffButtons;

        return true;
    }

    private bool UnsubscribeFromDispencer()
    {
        if (_dispencer == null)
        {
            return false;
        }

        _dispencer.Cleared -= OffButtons;

        return true;
    }

    private void OnButtons()
    {
        _addButton.BecomeActive();
        _takeButton.BecomeActive();
        _switchButton.BecomeActive();
    }

    private void OffButtons()
    {
        _dispencerSubscriber?.Unsubscribe();

        _addButton.BecomeInactive();
        _takeButton.BecomeInactive();
        _switchButton.BecomeInactive();

        _dispencer = null;
    }

    private void SendCommand(Action action, float delay)
    {
        _currentCommand = new Command(action, delay);

        CommandCreated?.Invoke(_currentCommand);
    }

    private void CancelCommand()
    {
        _currentCommand?.Cancel();

        _currentCommand = null;
    }
}