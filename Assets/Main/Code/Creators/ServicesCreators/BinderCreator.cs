public class BinderCreator
{
    public ModelPresenterBinder Create(IModelPresenterCreator modelPresenterCreator)
    {
        return new ModelPresenterBinder(modelPresenterCreator);
    }
}