public abstract class PresenterFactory<P> : MonoBehaviourFactory<P>, IPresenterCreator where P : Presenter
{
    public virtual Presenter CreatePresenter()
    {
        return Create();
    }
}