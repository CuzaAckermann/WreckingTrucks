using System;

public class SwapAbilityInputHandler
{
    private readonly IElementInput _interactInput;

    public SwapAbilityInputHandler(IElementInput interactInput)
    {
        _interactInput = interactInput ?? throw new ArgumentNullException(nameof(interactInput));
    }

    public event Action InteractPressed;

    public void Update()
    {
        if (_interactInput.IsPressed())
        {
            InteractPressed?.Invoke();
        }
    }
}