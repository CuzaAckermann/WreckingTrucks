using UnityEngine;

public class TickEngineUpdater : MonoBehaviour
{
    [SerializeField, Range(0, 10)] private float _timeFlowCoefficient1;
    [SerializeField, Range(1, 10)] private float _timeFlowCoefficient2;

    private TickEngine _tickEngine;

    public void Initialize()
    {
        _tickEngine = new TickEngine();
        Pause();
    }

    private void Update()
    {
        _tickEngine.Tick(Time.deltaTime * _timeFlowCoefficient1 * _timeFlowCoefficient2);
    }

    public void AddTickable(ITickable tickable)
    {
        _tickEngine.AddTickable(tickable);
    }

    public void RemoveTickable(ITickable tickable)
    {
        _tickEngine.RemoveTickable(tickable);
    }

    public void Pause()
    {
        _tickEngine.Pause();
    }

    public void Continue()
    {
        _tickEngine.Continue();
    }
}