using System;
using System.Collections.Generic;
using UnityEngine;

public class NonstopGameWindow : MonoBehaviour
{
    [SerializeField] private GameButton _nonstopGameButton;
    [SerializeField] private GameButton _playButton;

    public GameButton NonstopGameButton => _nonstopGameButton;

    public GameButton PlayButton => _playButton;

    public void Enter()
    {
        _nonstopGameButton.BecomeInactive();

        _playButton.On();
    }

    public void Exit()
    {
        _playButton.Off();

        _nonstopGameButton.BecomeActive();
    }
}