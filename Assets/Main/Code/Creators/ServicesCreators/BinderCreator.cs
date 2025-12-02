using System;

public class BinderCreator
{
    private readonly PresenterProductionCreator _presenterProductionCreator;
    private readonly PresenterPainter _presenterPainter;
    private readonly ModelProductionCreator _modelProductionCreator;

    public BinderCreator(PresenterProductionCreator presenterProductionCreator,
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