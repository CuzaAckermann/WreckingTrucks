using UnityEngine;
using UnityEngine.UI;

public abstract class BaseButton : MonoBehaviour
{
    [SerializeField] private Button _button;

    private bool _isSubscribed = false;

    private void OnEnable()
    {
        SubscribeToButton();
    }

    private void OnDisable()
    {
        UnsubscribeFromButton();
    }

    public void On()
    {
        gameObject.SetActive(true);

        SubscribeToButton();
    }

    public void Off()
    {
        gameObject.SetActive(false);

        UnsubscribeFromButton();
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

    private void SubscribeToButton()
    {
        if (_isSubscribed == false)
        {
            _button.onClick.AddListener(OnPressed);

            _isSubscribed = true;
        }
    }

    private void UnsubscribeFromButton()
    {
        if (_isSubscribed)
        {
            _button.onClick.RemoveListener(OnPressed);

            _isSubscribed = false;
        }
    }
}