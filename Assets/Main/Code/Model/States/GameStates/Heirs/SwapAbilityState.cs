using System;

public class SwapAbilityState : GameState
{
    private readonly IPresenterDetector<BlockPresenter> _blockPresenterDetector;
    private readonly SwapAbilityInputHandler _inputHandler;
    private readonly BlockFieldManipulator _blockFieldManipulator;
    private readonly SwapAbility _swapAbility;

    private readonly BlockFieldCreator _blockFieldCreator;
    
    private Field _field;

    private bool _isSubscribed = false;

    public SwapAbilityState(IPresenterDetector<BlockPresenter> blockPresenterDetector,
                            SwapAbilityInputHandler inputHandler,
                            BlockFieldManipulator blockFieldManipulator,
                            SwapAbility swapAbility,
                            BlockFieldCreator blockFieldCreator)
    {
        _blockPresenterDetector = blockPresenterDetector ?? throw new ArgumentNullException(nameof(blockPresenterDetector));
        _inputHandler = inputHandler ?? throw new ArgumentNullException(nameof(inputHandler));
        _blockFieldManipulator = blockFieldManipulator ?? throw new ArgumentNullException(nameof(blockFieldManipulator));
        _swapAbility = swapAbility ?? throw new ArgumentNullException(nameof(swapAbility));
        _blockFieldCreator = blockFieldCreator ?? throw new ArgumentNullException(nameof(blockFieldCreator));

        SubscribeToBlockFieldCreator();
    }

    public event Action AbilityStarting;
    public event Action AbilityFinished;

    public override void Enter()
    {
        base.Enter();

        _field.Enable();
        _blockFieldManipulator.OpenField();

        _inputHandler.InteractPressed += OnInteractPressed;
    }

    public override void Update(float deltaTime)
    {
        _inputHandler.Update();
    }

    public override void Exit()
    {
        _inputHandler.InteractPressed -= OnInteractPressed;

        _field.Disable();
        _blockFieldManipulator.CloseField();

        base.Exit();
    }

    private void OnInteractPressed()
    {
        if (_blockPresenterDetector.TryGetPresenter(out BlockPresenter blockPresenter))
        {
            if (blockPresenter.Model is Block block)
            {
                _inputHandler.InteractPressed -= OnInteractPressed;

                AbilityStarting?.Invoke();

                // тут начинаем замену одного типа блоков на другой

            }
        }
    }

    private void OnAbilityStarting()
    {
        AbilityStarting?.Invoke();
    }

    private void OnAbilityFinished()
    {
        AbilityFinished?.Invoke();
    }

    private void SubscribeToBlockFieldCreator()
    {
        if (_isSubscribed == false)
        {
            _blockFieldCreator.BlockFieldCreated += OnBlockFieldCreated;

            _isSubscribed = true;
        }
    }

    private void UnsubscribeFromBlockFieldCreator()
    {
        if (_isSubscribed)
        {
            _blockFieldCreator.BlockFieldCreated -= OnBlockFieldCreated;

            _isSubscribed = false;
        }
    }

    private void OnBlockFieldCreated(Field field)
    {
        _field = field ?? throw new ArgumentNullException(nameof(field));
        _blockFieldManipulator.SetField(field);
        _swapAbility.SetField(field);
    }
}