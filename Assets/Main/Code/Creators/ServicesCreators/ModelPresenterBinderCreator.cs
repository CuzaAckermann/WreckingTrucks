using System;

public class ModelPresenterBinderCreator
{
    private readonly PresenterProductionCreator _presenterProductionCreator;
    private readonly PresenterPainter _presenterPainter;
    private readonly ModelProductionCreator _modelProductionCreator;

    public ModelPresenterBinderCreator(PresenterProductionCreator presenterProductionCreator,
                         PresenterPainter presenterPainter,
                         ModelProductionCreator modelProductionCreator)
    {
        _presenterProductionCreator = presenterProductionCreator ?
                                      presenterProductionCreator :
                                      throw new ArgumentNullException(nameof(presenterProductionCreator));
        _presenterPainter = presenterPainter ? presenterPainter : throw new ArgumentNullException(nameof(presenterPainter));
        _modelProductionCreator = modelProductionCreator ?? throw new ArgumentNullException(nameof(modelProductionCreator));
    }

    public ModelPresenterBinder Create()
    {
        return new ModelPresenterBinder(_modelProductionCreator.CreateModelProduction(),
                                        _presenterProductionCreator.Create(),
                                        _presenterPainter);
    }
}