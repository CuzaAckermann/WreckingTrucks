using UnityEngine;

public class Placer
{
    private readonly EventBus _eventBus;

    public Placer(EventBus eventBus)
    {
        Validator.ValidateNotNull(eventBus);

        _eventBus = eventBus;
    }

    public void Place(Model model, Vector3 position)
    {
        model.Placeable.SetPosition(position);

        _eventBus.Invoke(new PlaceableSignal(model));
    }
}