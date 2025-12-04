public class FinishedTruckDestroyer
{
    private readonly TriggerDetector<TruckPresenter> _truckPresenterTriggerDetector;

    public FinishedTruckDestroyer(GameObjectTriggerDetector triggerDetector)
    {
        _truckPresenterTriggerDetector = new TriggerDetector<TruckPresenter>(triggerDetector);

        SubscribeToDetector();
    }

    private void SubscribeToDetector()
    {
        _truckPresenterTriggerDetector.Detected += OnDetected;
        _truckPresenterTriggerDetector.Enable();
    }

    private void UnsubscribeFromDetector()
    {
        _truckPresenterTriggerDetector.Disable();
        _truckPresenterTriggerDetector.Detected -= OnDetected;
    }

    private void OnDetected(TruckPresenter truckPresenter)
    {
        truckPresenter.Model.Destroy();
    }
}