using UnityEngine;

public class ShopWindow : StateWindow<ShopInputState>
{
    [SerializeField] private GameButton _returnButton;

    public GameButton ReturnButton => _returnButton;
}