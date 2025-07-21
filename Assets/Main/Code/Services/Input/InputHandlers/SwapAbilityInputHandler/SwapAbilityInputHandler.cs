using System;

public class SwapAbilityInputHandler
{
    private readonly IInteractInput _interactInput;

    public SwapAbilityInputHandler(IInteractInput interactInput)
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