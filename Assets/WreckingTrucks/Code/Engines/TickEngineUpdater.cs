using UnityEngine;

public class TickEngineUpdater : MonoBehaviour, ITickEngineUpdaterOnlyAddAndRemove
{
    [SerializeField, Range(0, 1)] private float _slowTimeFlowCoefficient = 1;
    [SerializeField, Range(1, 10)] private float _mediumTimeFlowCoefficient = 1;
    [SerializeField, Range(1, 10)] private float _hardTimeFlowCoefficient = 1;

    private TickEngine _tickEngine;

    public void Initialize()
    {
        _tickEngine = new TickEngine();
    }

    private void Update()
    {
        _tickEngine.Tick(Time.deltaTime * _slowTimeFlowCoefficient
                                        * _mediumTimeFlowCoefficient
                                        * _hardTimeFlowCoefficient);
    }

    public void Add(ITickable tickable)
    {
        _tickEngine.AddTickable(tickable);
    }

    public void Remove(ITickable tickable)
    {
        _tickEngine.RemoveTickable(tickable);
    }

    public void Clear()
    {
        _tickEngine.Clear();
    }

    public void Switch()
    {
        _tickEngine.Switch();
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