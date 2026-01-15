using UnityEngine;

public class GlobalTime : MonoBehaviour
{
    [Header("Displayers")]
    [SerializeField] private TimeDisplay _timeDisplay;
    [SerializeField] private RoundedAmountDisplay _deltaTimeDisplay;

    public void Init(Stopwatch stopwatch, DeltaTimeCoefficientDefiner deltaTimeCoefficientDefiner)
    {
        _timeDisplay.Off();
        _deltaTimeDisplay.Off();

        _timeDisplay.Init(stopwatch);
        _deltaTimeDisplay.Init(deltaTimeCoefficientDefiner);

        StartCounting(stopwatch);
    }

    private void StartCounting(Stopwatch stopwatch)
    {
        _timeDisplay.On();
        _deltaTimeDisplay.On();

        stopwatch?.Start();
    }
}