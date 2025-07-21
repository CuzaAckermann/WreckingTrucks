using System;

public class SwapAbilityState : GameState
{
    private readonly IPresenterDetector<BlockPresenter> _blockPresenterDetector;
    private readonly SwapAbilityInputHandler _inputHandler;

    public SwapAbilityState(IPresenterDetector<BlockPresenter> blockPresenterDetector,
                            SwapAbilityInputHandler inputHandler)
    {
        _blockPresenterDetector = blockPresenterDetector ?? throw new ArgumentNullException(nameof(blockPresenterDetector));
        _inputHandler = inputHandler ?? throw new ArgumentNullException(nameof(inputHandler));
    }

    public event Action AbilityStarting;
    public event Action AbilityFinished;

    public void Prepare()
    {

    }

    public override void Enter()
    {
        base.Enter();

        _inputHandler.InteractPressed += OnInteractPressed;
    }

    public override void Update(float deltaTime)
    {
        _inputHandler.Update();
    }

    public override void Exit()
    {
        _inputHandler.InteractPressed -= OnInteractPressed;

        base.Exit();
    }

    private void OnInteractPressed()
    {
        if (_blockPresenterDetector.TryGetPresenter(out BlockPresenter blockPresenter))
        {
            // тут начинаем замену одного типа блоков на другой
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