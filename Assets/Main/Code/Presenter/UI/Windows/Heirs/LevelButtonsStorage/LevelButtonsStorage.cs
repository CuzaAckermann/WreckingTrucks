using System;
using UnityEngine;

public class LevelButtonsStorage : WindowOfState<LevelSelectionInputState>
{
    [SerializeField] private GameButton _returnButton;

    [Header("Subwindows")]
    [SerializeField] private ButtonsSlider _buttonsSlider;
    [SerializeField] private NonstopGameWindow _nonstopGameWindow;

    private bool _isInitialized = false;
    
    public void Init(LevelSelectionInputState levelSelectionState, float animationSpeed, int amountLevels)
    {
        if (_isInitialized)
        {
            throw new InvalidOperationException($"Already initialized");
        }

        base.Init(levelSelectionState, animationSpeed);

        _buttonsSlider.Init(amountLevels);

        ShowCurrentGrid();

        _isInitialized = true;
    }

    public GameButton ReturnButton => _returnButton;

    public GameButton PlayButtonNonstopGame => _nonstopGameWindow.PlayButton;

    public bool TryGetButton(int index, out ButtonWithIndex buttonWithIndex)
    {
        return _buttonsSlider.TryGetButton(index, out buttonWithIndex);
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

    private void SubscribeToWindows()
    {
        _buttonsSlider.SubscribeToNavigationButtons();

        _buttonsSlider.LevelsButton.Pressed += ShowCurrentGrid;
        _nonstopGameWindow.NonstopGameButton.Pressed += ShowPlayButtonNonstopGame;
    }

    public void UnsubscribeFromWindows()
    {
        _buttonsSlider.UnsubscribeFromNavigationButtons();

        _buttonsSlider.LevelsButton.Pressed -= ShowCurrentGrid;
        _nonstopGameWindow.NonstopGameButton.Pressed -= ShowPlayButtonNonstopGame;
    }

    private void ShowCurrentGrid()
    {
        _nonstopGameWindow.Exit();

        _buttonsSlider.Enter();
    }

    private void ShowPlayButtonNonstopGame()
    {
        _buttonsSlider.Exit();

        _nonstopGameWindow.Enter();
    }
}