using System;

public class ModelPresenterBinderCreator
{
    private readonly PresenterProductionCreator _presenterProductionCreator;
    private readonly PresenterPainter _presenterPainter;

    public ModelPresenterBinderCreator(PresenterProductionCreator presenterProductionCreator,
                         PresenterPainter presenterPainter)
    {
        _presenterProductionCreator = presenterProductionCreator ?
                                      presenterProductionCreator :
                                      throw new ArgumentNullException(nameof(presenterProductionCreator));
        _presenterPainter = presenterPainter ? presenterPainter : throw new ArgumentNullException(nameof(presenterPainter));
    }

    public ModelPresenterBinder Create(ApplicationStateStorage applicationStateStorage,
                                       ModelProduction modelProduction)
    {
        return new ModelPresenterBinder(applicationStateStorage,
                                        modelProduction,
                                        _presenterProductionCreator.Create(),
                                        _presenterPainter);
    }
}