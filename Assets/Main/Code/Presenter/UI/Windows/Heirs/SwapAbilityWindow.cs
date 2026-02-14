using UnityEngine;

public class SwapAbilityWindow : WindowOfState<SwapAbilityInputState>
{
    [SerializeField] private GameButton _returnButton;

    public GameButton ReturnButton => _returnButton;

    public override void Init(SwapAbilityInputState gameState, float animationSpeed)
    {
        _returnButton.BecomeActive();

        base.Init(gameState, animationSpeed);
    }

    public void OffReturnButton()
    {
        // Нужно отключать кнопку возврата когда началась замена
        //swapAbilityState.AbilityStarting += OffReturnButton;

        _returnButton.BecomeInactive();
    }
}