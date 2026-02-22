public class FinishedTruckDestroyer : IApplicationAbility
{
    private readonly TriggerDetector<TruckPresenter> _truckPresenterTriggerDetector;

    public FinishedTruckDestroyer(GameObjectTriggerDetector triggerDetector)
    {
        _truckPresenterTriggerDetector = new TriggerDetector<TruckPresenter>(triggerDetector);
    }

    public void Start()
    {
        _truckPresenterTriggerDetector.Detected += OnDetected;
        _truckPresenterTriggerDetector.Enable();
    }

    public void Finish()
    {
        _truckPresenterTriggerDetector.Disable();
        _truckPresenterTriggerDetector.Detected -= OnDetected;
    }

    private void OnDetected(TruckPresenter truckPresenter)
    {
        if (truckPresenter == null)
        {
            //Logger.Log("TYT NUll");

            return;
        }

        if (truckPresenter.Model == null)
        {
            //Logger.Log("Model is null");

            return;
        }

        truckPresenter.Model.Destroy();
    }
}