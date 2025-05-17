using UnityEngine;

public class EndLevelWindow : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;

    public void ShowWindow()
    {
        _canvasGroup.alpha = 1;
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.interactable = true;
    }

    public void HideWindow()
    {
        _canvasGroup.alpha = 0;
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.interactable = false;
    }
}