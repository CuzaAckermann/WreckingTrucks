using UnityEngine;

public class GlobalTime : MonoBehaviour
{
    [Header("Displayers")]
    [SerializeField] private TimeDisplay _timeDisplay;
    [SerializeField] private RoundedAmountDisplay _deltaTimeFactorDisplay;

    public void Init(Stopwatch stopwatch, IAmount deltaTimeFactor)
    {
        Validator.ValidateNotNull(stopwatch);

        _timeDisplay.Off();
        _deltaTimeFactorDisplay.Off();

        _timeDisplay.Init(stopwatch.Time);
        _deltaTimeFactorDisplay.Init(deltaTimeFactor);

        StartCounting(stopwatch);
    }

    private void StartCounting(Stopwatch stopwatch)
    {
        _timeDisplay.On();
        _deltaTimeFactorDisplay.On();

        stopwatch.Start();
    }
}