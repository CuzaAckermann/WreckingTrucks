using UnityEngine;
using UnityEngine.UI;

public abstract class BaseButton : MonoBehaviour
{
    [SerializeField] private Button _button;

    private void OnEnable()
    {
        _button.onClick.AddListener(OnPressed);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnPressed);
    }

    public void On()
    {
        _button.interactable = true;
    }

    public void Off()
    {
        _button.interactable = false;
    }

    protected abstract void OnPressed();
}