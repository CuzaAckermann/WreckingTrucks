using UnityEngine;

public abstract class WindowOfState<IS> : MonoBehaviourSubscriber where IS : InputState<IInput>
{
    [SerializeField] private CanvasGroup _canvasGroup;

    private IS _inputState;

    private WindowAnimation _windowAnimation;

    public virtual void Init(IS inputState, float animationSpeed)
    {
        Validator.ValidateNotNull(inputState);

        _inputState = inputState;

        Init();

        _windowAnimation = new WindowAnimation(_canvasGroup, animationSpeed);
    }

    public virtual void Show()
    {
        //_windowAnimation.StartShow();

        _canvasGroup.alpha = 1;
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.interactable = true;
    }

    public void Hide()
    {
        //_windowAnimation.StartHide();

        _canvasGroup.alpha = 0;
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.interactable = false;
    }

    protected override void Subscribe()
    {
        _inputState.Entered += Show;
        _inputState.Exited += Hide;
    }

    protected override void Unsubscribe()
    {
        _inputState.Entered -= Show;
        _inputState.Exited -= Hide;
    }
}