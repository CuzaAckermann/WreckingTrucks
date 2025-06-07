public interface IPresenterDetector<T> where T : Presenter
{
    public bool TryGetPresenter(out T presenter);
}