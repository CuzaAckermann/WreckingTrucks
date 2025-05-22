using UnityEngine;
using UnityEditor;

public class TestGame : MonoBehaviour
{
    [Header("Settings Time")]
    [SerializeField, Range(0.01f, 10)] private float _currentTimeScale = 5f;

    [Header("Settings FrameRate")]
    [SerializeField, Range(30, 300)] private int _targetFrameRate = 60;
    [SerializeField] private int _maxTargetFrameRate = 305;
    [SerializeField] private int _increaseTargetFrameRate = 5;

    [Header("Settings Interactable")]
    [SerializeField] private ResetButton _resetButton;

    [Header("Settings Stopwatch")]
    [SerializeField, Min(7)] private float _notificationInterval;

    private Stopwatch _stopwatch;

    private void Awake()
    {
        Application.targetFrameRate = _targetFrameRate;
        _stopwatch = new Stopwatch(_notificationInterval);
        _stopwatch.Start();
    }

    private void OnEnable()
    {
        _stopwatch.IntervalPassed += OnPressResetButton;
    }

    private void Update()
    {
        Time.timeScale = _currentTimeScale;
        _stopwatch.Tick(Time.deltaTime);
    }

    private void OnDisable()
    {
        _stopwatch.IntervalPassed -= OnPressResetButton;
    }

    private void OnPressResetButton()
    {
#if UNITY_EDITOR
        if (_targetFrameRate >= _maxTargetFrameRate)
        {
            EditorApplication.isPlaying = false;
        }
#endif

        _targetFrameRate += _increaseTargetFrameRate;
        Application.targetFrameRate = _targetFrameRate;
        _resetButton.OnReset();
        _stopwatch.Start();
    }
}