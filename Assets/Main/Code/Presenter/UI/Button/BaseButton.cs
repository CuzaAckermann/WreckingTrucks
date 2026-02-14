using UnityEngine;
using UnityEngine.UI;

public abstract class BaseButton : MonoBehaviourSubscriber<Button>
{
    [SerializeField] private Button _button;

    private void Awake()
    {
        Init(_button);
    }

    public void Switch(bool needActivate)
    {
        if (needActivate)
        {
            On();
        }
        else
        {
            Off();
        }
    }

    public void BecomeActive()
    {
        _button.interactable = true;
    }

    public void BecomeInactive()
    {
        _button.interactable = false;
    }

    protected abstract void OnPressed();

    protected override void SubscribeToNotifier(Button notifier)
    {
        _button.onClick.AddListener(OnPressed);
    }

    protected override void UnsubscribeFromNotifier(Button notifier)
    {
        _button.onClick.RemoveListener(OnPressed);
    }
}