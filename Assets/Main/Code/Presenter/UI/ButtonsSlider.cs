using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsSlider : MonoBehaviour, ITickable
{
    [Header("Windows")]
    [SerializeField] private RectTransform _viewWindow;
    [SerializeField] private RectTransform _content;

    [Header("Animation")]
    [SerializeField] private AnimationCurve _movementCurve;
    [SerializeField] private float _animationSpeed;

    [Header("Navigation")]
    [SerializeField] private GameButton _levelsButton;
    [SerializeField] private GameButton _previousLevels;
    [SerializeField] private GameButton _nextLevels;

    [Header("Width Calculation")]
    [SerializeField] private GridLayoutGroup _layoutGroup;

    [Header("Buttons")]
    [SerializeField] private List<ButtonWithIndex> _buttonsWithNumbers;

    private int _windowsAmount;

    private int _indexOfCurrentWindow = 0;

    private float _progress = 0;

    private int _targetPositionX;
    private float _distance;

    private bool _isSubscribedToNavigationButtons = false;

    public void Init(int levelAmount)
    {
        if (_buttonsWithNumbers.Count == 0)
        {
            throw new InvalidOperationException($"{nameof(_buttonsWithNumbers)} is empty");
        }

        DefineAmountWindows();

        NumberButtons();

        DisableButtons(levelAmount);
        
        DefineButtonsNavigation();
    }

    public event Action<ITickable> Activated;
    public event Action<ITickable> Deactivated;
    public event Action<IDestroyable> Destroyed;

    public GameButton LevelsButton => _levelsButton;

    public void Destroy()
    {
        Destroyed?.Invoke(this);
    }

    public void SetInteractable(int index)
    {
        if (index < 0 || index >= _buttonsWithNumbers.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }

        _buttonsWithNumbers[index].BecomeActive();
    }

    public void Enter()
    {
        _levelsButton.BecomeInactive();

        //_viewWindow.StartShow();
        _viewWindow.gameObject.SetActive(true);

        DefineButtonsNavigation();
    }

    public void Tick(float deltaTime)
    {
        Vector2 contentPosition = _content.position;

        float step = _animationSpeed * deltaTime;
        _progress += Mathf.Abs(step / _distance);

        contentPosition = new Vector2(Mathf.Lerp(contentPosition.x, _targetPositionX, _movementCurve.Evaluate(_progress)),
                                      contentPosition.y);

        _content.position = contentPosition;

        if ((int)_content.position.x == _targetPositionX)
        {
            _progress = 1;

            Deactivated?.Invoke(this);

            _previousLevels.BecomeActive();
            _nextLevels.BecomeActive();
        }
    }

    public void Exit()
    {
        _levelsButton.BecomeActive();

        //_viewWindow.StartHide();
        _viewWindow.gameObject.SetActive(false);

        _previousLevels.Off();
        _nextLevels.Off();
    }

    public bool TryGetButton(int number, out ButtonWithIndex buttonWithNumber)
    {
        buttonWithNumber = null;

        for (int currentButton = 0; currentButton < _buttonsWithNumbers.Count; currentButton++)
        {
            if (_buttonsWithNumbers[currentButton].Index == number)
            {
                buttonWithNumber = _buttonsWithNumbers[currentButton];

                break;
            }
        }

        return buttonWithNumber != null;
    }

    public void SubscribeToNavigationButtons()
    {
        if (_isSubscribedToNavigationButtons == false)
        {
            _previousLevels.Pressed += SwitchPreviousWindow;
            _nextLevels.Pressed += SwitchNextWindow;

            _isSubscribedToNavigationButtons = true;
        }
    }

    public void UnsubscribeFromNavigationButtons()
    {
        if (_isSubscribedToNavigationButtons)
        {
            _previousLevels.Pressed -= SwitchPreviousWindow;
            _nextLevels.Pressed -= SwitchNextWindow;

            _isSubscribedToNavigationButtons = false;
        }
    }

    private void DisableButtons(int startIndex)
    {
        for (int currentButton = startIndex; currentButton < _buttonsWithNumbers.Count; currentButton++)
        {
            _buttonsWithNumbers[currentButton].BecomeInactive();
        }
    }

    private void SwitchPreviousWindow()
    {
        SetIndexOfCurrentWindow(_indexOfCurrentWindow - 1);
    }

    private void SwitchNextWindow()
    {
        SetIndexOfCurrentWindow(_indexOfCurrentWindow + 1);
    }

    private void DefineAmountWindows()
    {
        float contentWidth = (_buttonsWithNumbers.Count / _layoutGroup.constraintCount) * _layoutGroup.cellSize.x;

        _windowsAmount = Mathf.CeilToInt(contentWidth / _viewWindow.sizeDelta.x);
    }

    private void SetIndexOfCurrentWindow(int index)
    {
        DefineTarget(index);

        _indexOfCurrentWindow = index;

        _previousLevels.BecomeInactive();
        _nextLevels.BecomeInactive();

        DefineButtonsNavigation();

        _progress = 0;
        _distance = _targetPositionX - _content.position.x;

        Activated?.Invoke(this);
    }

    private void DefineTarget(int index)
    {
        if (index > _indexOfCurrentWindow)
        {
            _targetPositionX = (int)(_content.position.x - _viewWindow.sizeDelta.x);
        }
        else if (index < _indexOfCurrentWindow)
        {
            _targetPositionX = (int)(_content.position.x + _viewWindow.sizeDelta.x);
        }
    }

    private void DefineButtonsNavigation()
    {
        _previousLevels.Switch(HasPreviousWindow());
        _nextLevels.Switch(HasNextWindow());
    }

    private bool HasPreviousWindow()
    {
        return _indexOfCurrentWindow - 1 >= 0;
    }

    private bool HasNextWindow()
    {
        return _indexOfCurrentWindow + 1 < _windowsAmount;
    }

    private void NumberButtons()
    {
        for (int currentButton = 0; currentButton < _buttonsWithNumbers.Count; currentButton++)
        {
            _buttonsWithNumbers[currentButton].SetIndex(currentButton);
        }
    }
}