public class ModelPresenterBinderCreator
{
    private readonly PresenterProductionCreator _presenterProductionCreator;
    private readonly PresenterPainter _presenterPainter;

    public ModelPresenterBinderCreator(PresenterProductionCreator presenterProductionCreator,
                                       PresenterPainter presenterPainter)
    {
        Validator.ValidateNotNull(presenterProductionCreator, presenterPainter);

        _presenterProductionCreator = presenterProductionCreator;
        _presenterPainter = presenterPainter;
    }

    public ModelPresenterBinder Create(EventBus eventBus)
    {
        return new ModelPresenterBinder(eventBus,
                                        _presenterProductionCreator.Create(),
                                        _presenterPainter);
    }
}