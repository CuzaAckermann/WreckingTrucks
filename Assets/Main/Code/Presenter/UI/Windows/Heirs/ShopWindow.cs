using UnityEngine;

public class ShopWindow : WindowOfState<ShopInputState>
{
    [SerializeField] private GameButton _returnButton;

    public GameButton ReturnButton => _returnButton;
}