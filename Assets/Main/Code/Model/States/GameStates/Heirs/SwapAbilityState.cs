using System;

public class SwapAbilityState : GameState
{
    private readonly IPresenterDetector<BlockPresenter> _blockPresenterDetector;
    private readonly SwapAbilityInputHandler _inputHandler;
    private readonly BlockFieldManipulator _blockFieldManipulator;
    private readonly SwapAbility _swapAbility;

    private readonly EventBus _eventBus;
    
    private Field _blockField;

    public SwapAbilityState(IPresenterDetector<BlockPresenter> blockPresenterDetector,
                            SwapAbilityInputHandler inputHandler,
                            BlockFieldManipulator blockFieldManipulator,
                            SwapAbility swapAbility,
                            EventBus eventBus)
    {
        _blockPresenterDetector = blockPresenterDetector ?? throw new ArgumentNullException(nameof(blockPresenterDetector));
        _inputHandler = inputHandler ?? throw new ArgumentNullException(nameof(inputHandler));
        _blockFieldManipulator = blockFieldManipulator ?? throw new ArgumentNullException(nameof(blockFieldManipulator));
        _swapAbility = swapAbility ?? throw new ArgumentNullException(nameof(swapAbility));
        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));

        _eventBus.Subscribe<CreatedBlockFieldSignal>(SetBlockField);
    }

    public event Action AbilityStarting;
    public event Action AbilityFinished;

    public override void Enter()
    {
        base.Enter();

        _blockField.Enable(new EnabledGameWorldSignal());
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

        _blockField.Disable(new DisabledGameWorldSignal());
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

    private void SetBlockField(CreatedBlockFieldSignal createdBlockFieldSignal)
    {
        BlockField blockField = createdBlockFieldSignal.BlockField;

        _blockField = blockField ?? throw new ArgumentNullException(nameof(blockField));
        _blockFieldManipulator.SetField(blockField);
        _swapAbility.SetField(blockField);
    }
}