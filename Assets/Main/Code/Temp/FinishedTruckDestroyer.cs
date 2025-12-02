using System;

public class FinishedTruckDestroyer
{
    private readonly TriggerTruckPresenterDetector _triggerTruckPresenterDetector;

    public FinishedTruckDestroyer(TriggerTruckPresenterDetector triggerTruckPresenterDetector)
    {
        _triggerTruckPresenterDetector = triggerTruckPresenterDetector ? triggerTruckPresenterDetector : throw new ArgumentNullException(nameof(triggerTruckPresenterDetector));

        SubscribeToDetector();
    }

    private void SubscribeToDetector()
    {
        _triggerTruckPresenterDetector.Detected += OnDetected;
    }

    private void UnsubscribeFromDetector()
    {
        _triggerTruckPresenterDetector.Detected -= OnDetected;
    }

    private void OnDetected(TruckPresenter truckPresenter)
    {
        truckPresenter.Model.Destroy();
    }
}