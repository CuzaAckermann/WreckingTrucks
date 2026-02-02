using UnityEngine;

public class ShopWindow : WindowOfState<ShopState>
{
    [SerializeField] private GameButton _returnButton;

    public GameButton ReturnButton => _returnButton;
}