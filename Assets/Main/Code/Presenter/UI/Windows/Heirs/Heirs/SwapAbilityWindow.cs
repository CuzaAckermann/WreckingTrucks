using UnityEngine;

public class SwapAbilityWindow : StateWindow<SwapAbilityInputState>
{
    [SerializeField] private GameButton _returnButton;

    public GameButton ReturnButton => _returnButton;

    public void Init()
    {
        _returnButton.BecomeActive();
    }

    public void OffReturnButton()
    {
        // Нужно отключать кнопку возврата когда началась замена
        //swapAbilityState.AbilityStarting += OffReturnButton;

        _returnButton.BecomeInactive();
    }
}