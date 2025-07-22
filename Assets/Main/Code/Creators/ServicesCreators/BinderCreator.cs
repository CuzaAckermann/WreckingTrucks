using System;

public class BinderCreator
{
    private readonly PresenterProductionCreator _presenterProductionCreator;

    public BinderCreator(PresenterProductionCreator presenterProductionCreator)
    {
        _presenterProductionCreator = presenterProductionCreator ?
                                      presenterProductionCreator :
                                      throw new ArgumentNullException(nameof(presenterProductionCreator));
    }

    public ModelPresenterBinder Create()
    {
        return new ModelPresenterBinder(_presenterProductionCreator.Create());
    }
}