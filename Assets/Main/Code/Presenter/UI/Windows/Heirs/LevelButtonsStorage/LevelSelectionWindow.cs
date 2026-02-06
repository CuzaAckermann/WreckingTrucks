using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectionWindow : MonoBehaviour
{
    [SerializeField] private List<LevelButtonsGrid> _levelButtonsGrids;

    [Header("Navigation")]
    [SerializeField] private GameButton _previousLevels;
    [SerializeField] private GameButton _nextLevels;
    [SerializeField] private GameButton _levelsButton;

    private LevelButtonsGrid _currentLevelButtonsGrid;
    
    private int _amountLevels;

    private bool _isSubscribedToNavigationButtons = false;

    public void Init(int amountLevels)
    {
        if (_levelButtonsGrids.Count == 0)
        {
            throw new InvalidOperationException($"{nameof(_levelButtonsGrids)} is empty");
        }

        if (amountLevels <= 0 || amountLevels > GetAmountButtons())
        {
            throw new ArgumentOutOfRangeException(nameof(amountLevels));
        }

        _amountLevels = amountLevels;

        NumberButtons();

        HideGrids();

        DisableRemain();

        SetIndexOfCurrentGrid(0);
    }

    public GameButton LevelsButton => _levelsButton;

    public void Enter()
    {
        _levelsButton.BecomeInactive();

        DefineButtonsNavigation();

        _currentLevelButtonsGrid.gameObject.SetActive(true);
    }

    public void Exit()
    {
        _levelsButton.BecomeActive();

        _previousLevels.Off();
        _nextLevels.Off();

        _currentLevelButtonsGrid.gameObject.SetActive(false);
    }

    public bool TryGetButton(int index, out ButtonWithNumber buttonWithIndex)
    {
        buttonWithIndex = null;

        for (int currentGrid = 0; currentGrid < _levelButtonsGrids.Count; currentGrid++)
        {
            while (_levelButtonsGrids[currentGrid].TryGetByNumber(index + 1, out buttonWithIndex))
            {
                return true;
            }
        }

        return false;
    }

    public void SubscribeToNavigationButtons()
    {
        if (_isSubscribedToNavigationButtons == false)
        {
            _previousLevels.Pressed += SwitchPreviousGrid;
            _nextLevels.Pressed += SwitchNextGrid;

            _isSubscribedToNavigationButtons = true;
        }
    }

    public void UnsubscribeFromNavigationButtons()
    {
        if (_isSubscribedToNavigationButtons)
        {
            _previousLevels.Pressed -= SwitchPreviousGrid;
            _nextLevels.Pressed -= SwitchNextGrid;

            _isSubscribedToNavigationButtons = false;
        }
    }

    private void SwitchPreviousGrid()
    {
        SetIndexOfCurrentGrid(_levelButtonsGrids.IndexOf(_currentLevelButtonsGrid) - 1);
    }

    private void SwitchNextGrid()
    {
        SetIndexOfCurrentGrid(_levelButtonsGrids.IndexOf(_currentLevelButtonsGrid) + 1);
    }

    private void SetIndexOfCurrentGrid(int index)
    {
        if (_currentLevelButtonsGrid != null)
        {
            _currentLevelButtonsGrid.gameObject.SetActive(false);
        }

        _currentLevelButtonsGrid = _levelButtonsGrids[index];

        _currentLevelButtonsGrid.gameObject.SetActive(true);

        DefineButtonsNavigation();
    }

    private void DefineButtonsNavigation()
    {
        _previousLevels.Switch(HasPreviousGrid());
        _nextLevels.Switch(HasNextGrid());
    }

    private void NumberButtons()
    {
        int number = 1;

        for (int currentGrid = 0; currentGrid < _levelButtonsGrids.Count; currentGrid++)
        {
            LevelButtonsGrid levelButtonsGrid = _levelButtonsGrids[currentGrid];

            for (int currentButton = 0; currentButton < levelButtonsGrid.AmountButtons; currentButton++)
            {
                if (levelButtonsGrid.TryGetByIndex(currentButton, out ButtonWithNumber buttonWithNumber))
                {
                    buttonWithNumber.SetNumber(number);

                    number++;
                }
            }
        }
    }

    private void HideGrids()
    {
        for (int currentGrid = 0; currentGrid < _levelButtonsGrids.Count; currentGrid++)
        {
            _levelButtonsGrids[currentGrid].gameObject.SetActive(false);
        }
    }

    private int GetAmountButtons()
    {
        int amountButtons = 0;

        for (int currentGrid = 0; currentGrid < _levelButtonsGrids.Count; currentGrid++)
        {
            amountButtons += _levelButtonsGrids[currentGrid].AmountButtons;
        }

        return amountButtons;
    }

    private bool HasPreviousGrid()
    {
        return _levelButtonsGrids.IndexOf(_currentLevelButtonsGrid) - 1 >= 0;
    }

    private bool HasNextGrid()
    {
        return _levelButtonsGrids.IndexOf(_currentLevelButtonsGrid) + 1 < _levelButtonsGrids.Count;
    }

    private void DisableRemain()
    {
        int levelButtonsRemain = _amountLevels;

        int currentIndexOfGrid = -1;
        int currentButtonWithNumber = -1;

        bool isBreak = false;

        for (int currentGrid = 0; currentGrid < _levelButtonsGrids.Count && isBreak == false; currentGrid++)
        {
            LevelButtonsGrid levelButtonsGrid = _levelButtonsGrids[currentGrid];

            for (int currentButton = 0; currentButton < levelButtonsGrid.AmountButtons; currentButton++)
            {
                if (levelButtonsRemain == 0)
                {
                    currentIndexOfGrid = currentGrid;
                    currentButtonWithNumber = currentButton;

                    isBreak = true;

                    break;
                }

                levelButtonsRemain--;
            }
        }

        while (currentIndexOfGrid < _levelButtonsGrids.Count)
        {
            LevelButtonsGrid levelButtonsGrid = _levelButtonsGrids[currentIndexOfGrid];

            while (currentButtonWithNumber < levelButtonsGrid.AmountButtons)
            {
                if (levelButtonsGrid.TryGetByIndex(currentButtonWithNumber, out ButtonWithNumber buttonWithNumber))
                {
                    buttonWithNumber.BecomeInactive();
                }
                else
                {
                    Logger.Log("Trying is fail");
                }

                currentButtonWithNumber++;
            }

            currentButtonWithNumber = 0;
            currentIndexOfGrid++;
        }
    }
}