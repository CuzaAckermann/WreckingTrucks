public interface IModelPresenterCreator
{
    public bool TryGetPresenter(Model model, out Presenter presenter);
}