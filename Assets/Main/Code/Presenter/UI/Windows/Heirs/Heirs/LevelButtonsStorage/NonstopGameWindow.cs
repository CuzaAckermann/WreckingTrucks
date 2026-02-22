using TMPro;
using UnityEngine;

public class NonstopGameWindow : MonoBehaviour
{
    [SerializeField] private GameButton _nonstopGameButton;
    [SerializeField] private GameButton _playButton;
    [SerializeField] private TextMeshProUGUI _lastScore;

    public GameButton NonstopGameButton => _nonstopGameButton;

    public GameButton PlayButton => _playButton;

    public void Enter()
    {
        _nonstopGameButton.BecomeInactive();

        //StartShow();
        _lastScore.gameObject.SetActive(true);

        _playButton.On();
    }

    public void Exit()
    {
        _playButton.Off();

        //StartHide();
        _lastScore.gameObject.SetActive(false);

        _nonstopGameButton.BecomeActive();
    }
}