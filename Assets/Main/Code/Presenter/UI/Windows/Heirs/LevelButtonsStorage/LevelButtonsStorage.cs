using System;
using UnityEngine;

public class LevelButtonsStorage : WindowOfState<LevelSelectionState>
{
    [SerializeField] private GameButton _returnButton;

    [Header("Subwindows")]
    [SerializeField] private LevelSelectionWindow _levelSelectionWindow;
    [SerializeField] private NonstopGameWindow _nonstopGameWindow;

    private bool _isSubscribedToWindows = false;

    private bool _isInitialized = false;
    
    public void Init(LevelSelectionState levelSelectionState, int amountLevels)
    {
        if (_isInitialized)
        {
            throw new InvalidOperationException($"Already initialized");
        }

        base.Init(levelSelectionState);

        _levelSelectionWindow.Init(amountLevels);

        ShowCurrentGrid();

        _isInitialized = true;
    }

    public GameButton ReturnButton => _returnButton;

    public GameButton PlayButtonNonstopGame => _nonstopGameWindow.PlayButton;

    public bool TryGetButton(int index, out ButtonWithNumber buttonWithIndex)
    {
        return _levelSelectionWindow.TryGetButton(index, out buttonWithIndex);
    }

    protected override void Subscribe()
    {
        SubscribeToWindows();

        base.Subscribe();
    }

    protected override void Unsubscribe()
    {
        UnsubscribeFromWindows();

        base.Unsubscribe();
    }

    public void SubscribeToWindows()
    {
        if (_isSubscribedToWindows == false)
        {
            _levelSelectionWindow.SubscribeToNavigationButtons();

            _levelSelectionWindow.LevelsButton.Pressed += ShowCurrentGrid;
            _nonstopGameWindow.NonstopGameButton.Pressed += ShowPlayButtonNonstopGame;

            _isSubscribedToWindows = true;
        }
    }

    public void UnsubscribeFromWindows()
    {
        if (_isSubscribedToWindows)
        {
            _levelSelectionWindow.UnsubscribeFromNavigationButtons();

            _levelSelectionWindow.LevelsButton.Pressed -= ShowCurrentGrid;
            _nonstopGameWindow.NonstopGameButton.Pressed -= ShowPlayButtonNonstopGame;

            _isSubscribedToWindows = false;
        }
    }

    private void ShowCurrentGrid()
    {
        _nonstopGameWindow.Exit();

        _levelSelectionWindow.Enter();
    }

    private void ShowPlayButtonNonstopGame()
    {
        _levelSelectionWindow.Exit();

        _nonstopGameWindow.Enter();
    }
}