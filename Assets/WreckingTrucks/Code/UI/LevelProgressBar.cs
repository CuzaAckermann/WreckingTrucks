using System;
using UnityEngine;
using UnityEngine.UI;

// Õ≈ √Œ“Œ¬Œ
public class LevelProgressBar : MonoBehaviour
{
    [SerializeField] private Slider _progressBar;

    private const float MaxValue = 1;
    private const float MinValue = 0;

    private LevelProgress _levelProgress;

    public void Initialize(LevelProgress levelProgress)
    {
        UnsubscribeFromLevelProgress();
        _levelProgress = levelProgress ?? throw new ArgumentNullException(nameof(levelProgress));
        CustomizeSlider();
        OnCurrentValueChanged();
        SubscribeToLevelProgress();
    }

    public void OnEnable()
    {
        SubscribeToLevelProgress();
    }

    private void OnDisable()
    {
        UnsubscribeFromLevelProgress();
    }

    private void CustomizeSlider()
    {
        _progressBar.transition = Selectable.Transition.None;
        _progressBar.interactable = false;
        _progressBar.wholeNumbers = false;
        _progressBar.maxValue = MaxValue;
        _progressBar.minValue = MinValue;
    }

    private void SubscribeToLevelProgress()
    {
        if (_levelProgress != null)
        {
            _levelProgress.CurrentValueChanged += OnCurrentValueChanged;
        }
    }

    private void UnsubscribeFromLevelProgress()
    {
        if (_levelProgress != null)
        {
            _levelProgress.CurrentValueChanged -= OnCurrentValueChanged;
        }
    }

    private void OnCurrentValueChanged()
    {
        _progressBar.value = _levelProgress.CurrentValue;
    }
}