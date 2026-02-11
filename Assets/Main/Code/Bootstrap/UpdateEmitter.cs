using System;
using UnityEngine;

public class UpdateEmitter : MonoBehaviour
{
    private EventBus _eventBus;
    private IAmount _deltaTimeFactor;

    public void Init(EventBus eventBus,
                     IAmount deltaTimeFactor)
    {
        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));

        Validator.ValidateNotNull(deltaTimeFactor);
        _deltaTimeFactor = deltaTimeFactor;
    }

    private void Update()
    {
        _eventBus?.Invoke(new UpdateSignal(Time.deltaTime * _deltaTimeFactor.Value));
    }
}