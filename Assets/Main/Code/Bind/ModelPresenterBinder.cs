public class ModelPresenterBinder : IAbility
{
    private readonly EventBus _eventBus;
    private readonly IModelPresenterCreator _modelPresenterCreator;
    private readonly PresenterPainter _presenterPainter;

    public ModelPresenterBinder(EventBus eventBus,
                                IModelPresenterCreator modelPresenterCreators,
                                PresenterPainter presenterPainter)
    {
        Validator.ValidateNotNull(eventBus, modelPresenterCreators, presenterPainter);

        _eventBus = eventBus;
        _modelPresenterCreator = modelPresenterCreators;
        _presenterPainter = presenterPainter;
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

        if (_modelPresenterCreator.TryGetPresenter(model, out Presenter presenter) == false)
        {
            return;
        }

        presenter.Bind(model);
        _presenterPainter.Paint(presenter);
    }
}