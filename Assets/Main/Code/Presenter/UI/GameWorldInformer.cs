using UnityEngine;

public class GameWorldInformer : MonoBehaviour
{
    [SerializeField] private AmountDisplay _cartrigeBoxAmountDisplay;

    public void Initialize(IAmountChangedNotifier cartrigeBoxAmountNotifier)
    {
        _cartrigeBoxAmountDisplay.Initialize(cartrigeBoxAmountNotifier);
    }

    public void Show()
    {
        _cartrigeBoxAmountDisplay.On();
    }

    public void Hide()
    {
        _cartrigeBoxAmountDisplay.Off();
    }
}