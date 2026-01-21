using System;

public class SwapAbilityState : GameState
{
    private readonly SphereCastPresenterDetector _blockPresenterDetector;
    private readonly SwapAbilityInputHandler _inputHandler;
    private readonly BlockFieldManipulator _blockFieldManipulator;

    private readonly EventBus _eventBus;
    
    public SwapAbilityState(SphereCastPresenterDetector blockPresenterDetector,
                            SwapAbilityInputHandler inputHandler,
                            BlockFieldManipulator blockFieldManipulator,
                            EventBus eventBus)
    {
        _blockPresenterDetector = blockPresenterDetector ?? throw new ArgumentNullException(nameof(blockPresenterDetector));
        _inputHandler = inputHandler ?? throw new ArgumentNullException(nameof(inputHandler));
        _blockFieldManipulator = blockFieldManipulator ?? throw new ArgumentNullException(nameof(blockFieldManipulator));
        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
    }

    public event Action AbilityStarting;
    public event Action AbilityFinished;

    public override void Enter()
    {
        base.Enter();

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

        _blockFieldManipulator.CloseField();

        base.Exit();
    }

    private void OnInteractPressed()
    {
        if (_blockPresenterDetector.TryGetPresenter(out Presenter blockPresenter))
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
}