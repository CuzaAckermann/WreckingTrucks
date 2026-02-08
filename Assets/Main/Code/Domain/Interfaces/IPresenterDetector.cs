public interface IPresenterDetector<T> where T : IPresenter
{
    public bool TryGetPresenter(out T presenter);
}