using System;

public abstract class StateWindow<IS> : StateWindowBase where IS : InputState
{
    //[SerializeField] private CanvasGroup _canvasGroup;

    //private IS _inputState;

    //private WindowAnimation _windowAnimation;

    //public virtual void Init(IS inputState, float animationSpeed)
    //{
    //    Validator.ValidateNotNull(inputState);

    //    _inputState = inputState;

    //    //Init();

    //    _windowAnimation = new WindowAnimation(_canvasGroup, animationSpeed);
    //}

    public override Type BoundStateType => typeof(IS);

    //public float Value
    //{
    //    get
    //    {
    //        return _canvasGroup.alpha;
    //    }
    //    set
    //    {
    //        Validator.ValidateMin(value, 0, false);
    //        Validator.ValidateMax(value, 1, false);

    //        if (value == _canvasGroup.alpha)
    //        {
    //            return;
    //        }

    //        _canvasGroup.alpha = value;
    //    }
    //}

    //public virtual void Show()
    //{
    //    //_windowAnimation.StartShow();

    //    _canvasGroup.alpha = 1;
    //    _canvasGroup.blocksRaycasts = true;
    //    _canvasGroup.interactable = true;
    //}

    //public void Hide()
    //{
    //    //_windowAnimation.StartHide();

    //    _canvasGroup.alpha = 0;
    //    _canvasGroup.blocksRaycasts = false;
    //    _canvasGroup.interactable = false;
    //}

    //protected override void Subscribe()
    //{
    //    _inputState.Entered += Show;
    //    _inputState.Exited += Hide;
    //}

    //protected override void Unsubscribe()
    //{
    //    _inputState.Entered -= Show;
    //    _inputState.Exited -= Hide;
    //}
}