public class ModelPresenterBinderCreator
{
    private readonly Production _production;
    private readonly PresenterPainter _presenterPainter;

    public ModelPresenterBinderCreator(PresenterPainter presenterPainter,
                                       Production production)
    {
        Validator.ValidateNotNull(presenterPainter, production);

        _presenterPainter = presenterPainter;
        _production = production;
    }

    public ModelPresenterBinder Create(EventBus eventBus)
    {
        return new ModelPresenterBinder(eventBus,
                                        _production,
                                        _presenterPainter);
    }
}