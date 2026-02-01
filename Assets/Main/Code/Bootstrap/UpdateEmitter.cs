using System;
using UnityEngine;

public class UpdateEmitter : MonoBehaviour
{
    private EventBus _eventBus;
    private DeltaTimeCoefficientDefiner _deltaTimeCoefficientDefiner;

    public void Init(EventBus eventBus,
                     DeltaTimeCoefficientDefiner deltaTimeCoefficientDefiner)
    {
        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        _deltaTimeCoefficientDefiner = deltaTimeCoefficientDefiner ?? throw new ArgumentNullException(nameof(deltaTimeCoefficientDefiner));
    }

    private void Update()
    {
        _eventBus?.Invoke(new UpdateSignal(Time.deltaTime * _deltaTimeCoefficientDefiner.CurrentAmount));
    }
}