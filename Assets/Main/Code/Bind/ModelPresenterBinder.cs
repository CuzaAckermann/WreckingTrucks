using System;
using System.Collections.Generic;

public class ModelPresenterBinder : IAbility
{
    private readonly Dictionary<Type, Type> _bindingMap;

    private readonly EventBus _eventBus;
    private readonly Production _production;
    private readonly PresenterPainter _presenterPainter;

    public ModelPresenterBinder(EventBus eventBus,
                                Production production,
                                PresenterPainter presenterPainter)
    {
        Validator.ValidateNotNull(eventBus, production, presenterPainter);

        _eventBus = eventBus;
        _production = production;
        _presenterPainter = presenterPainter;

        _bindingMap = new Dictionary<Type, Type>()
        {
            { typeof(Block), typeof(BlockPresenter) },
            { typeof(Truck), typeof(TruckPresenter) },
            { typeof(Bullet), typeof(BulletPresenter) },
            { typeof(CartrigeBox), typeof(CartrigeBoxPresenter) },
            { typeof(Plane), typeof(PlanePresenter) }
        };
    }

    public void Start()
    {
        _eventBus.Subscribe<PlaceableSignal>(BindModelToPresenter);
    }

    public void Finish()
    {
        _eventBus.Unsubscribe<PlaceableSignal>(BindModelToPresenter);
    }

    private void BindModelToPresenter(PlaceableSignal placeableSignal)
    {
        Model model = placeableSignal.Model;

        Type modelType = model.GetType();

        if (_bindingMap.ContainsKey(modelType) == false)
        {
            return;
        }

        if (_production.TryCreate(_bindingMap[modelType], out IDestroyable destroyable) == false)
        {
            return;
        }

        if (Validator.IsRequiredType(destroyable, out Presenter presenter) == false)
        {
            throw new InvalidOperationException();
        }

        presenter.Bind(model);
        _presenterPainter.Paint(presenter);
    }
}