using UnityEngine;
using UnityEngine.UI;

public abstract class BaseUiButton : MonoBehaviourSubscriber
{
    [SerializeField] private Button _button;

    private void Awake()
    {
        Init();
    }

    public void Switch(bool needActivate)
    {
        if (needActivate)
        {
            gameObject.On();
        }
        else
        {
            gameObject.Off();
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

    protected override void Subscribe()
    {
        _button.onClick.AddListener(OnPressed);
    }

    protected override void Unsubscribe()
    {
        _button.onClick.RemoveListener(OnPressed);
    }
}